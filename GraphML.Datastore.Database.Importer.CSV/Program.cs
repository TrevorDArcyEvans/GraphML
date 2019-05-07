using CsvHelper.Configuration;
using Dapper.Contrib.Extensions;
using GraphML.Common;
using Microsoft.Extensions.Configuration;
using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GraphML.Datastore.Database.Importer.CSV
{
  public sealed class Program
  {
    static void Main(string[] args)
    {
      // check data file exists
      if (args.Length != 2 || !File.Exists(args[1]))
      {
        Usage();
        return;
      }

      new Program(args[0], args[1], LogInformation).Run();
    }

    private readonly string _repoName;
    private readonly string _dataFilePath;
    private readonly Action<string> _logInfoAction;

    public Program(string repoName, string dataFilePath, Action<string> logInfoAction)
    {
      _repoName = repoName;
      _dataFilePath = dataFilePath;
      _logInfoAction = logInfoAction;
    }

    public void Run()
    {
      var config = new ConfigurationBuilder()
        .AddJsonFile("hosting.json")
        .AddEnvironmentVariables()
        .Build();

      DumpSettings(config);
      _logInfoAction(Environment.NewLine);

      var sw = Stopwatch.StartNew();
      using (var tr = File.OpenText(_dataFilePath))
      {
        var csvCfg = new Configuration
        {
          Delimiter = Path.GetExtension(_dataFilePath).ToLowerInvariant() == ".csv" ? "," : "\t",
          AllowComments = true,
          HeaderValidated = null
        };
        csvCfg.RegisterClassMap<ImportEdgeClassMap>();
        using (var csv = new CsvHelper.CsvReader(tr, csvCfg))
        {
          var edges = csv.GetRecords<ImportEdge>().ToList();
          var nodes = edges.SelectMany(edge => new[] { edge.FromNode, edge.ToNode }).Distinct();

          var dbConnFact = new DbConnectionFactory(config);
          using (var conn = dbConnFact.Get())
          {
            var repo = conn.GetAll<Repository>().Single(r => r.Name == _repoName);
            var graph = new Graph(repo.Id, Path.GetFileNameWithoutExtension(_dataFilePath));

            _logInfoAction($"Importing from:  {_dataFilePath}");
            _logInfoAction($"          into:  {conn.ConnectionString}");
            _logInfoAction($"    repository:  {repo.Name}");
            _logInfoAction($"         graph:  {graph.Name}");
            _logInfoAction(Environment.NewLine);

            var modelNodes = nodes.Select(node => new Node(graph.Id, node)).ToList();
            var modelNodesMap = modelNodes.ToDictionary(node => node.Name);
            var modelEdges = edges.Select(edge =>
              new Edge(
                graph.Id,
                $"{edge.FromNode}-->{edge.ToNode}",
                modelNodesMap[edge.FromNode].Id,
                modelNodesMap[edge.ToNode].Id)).ToList();

            _logInfoAction($"Transformed data at         : {sw.ElapsedMilliseconds} ms");

            using (var trans = conn.BeginTransaction())
            {
              if (conn is SqlConnection && trans is SqlTransaction)
              {
                var bulky = new BulkUploadToMsSqlServer((SqlConnection)conn, (SqlTransaction)trans);

                bulky.Commit(new[] { graph }, GetTableName<Graph>());

                _logInfoAction($"Started node import at      : {sw.ElapsedMilliseconds} ms");
                bulky.Commit(modelNodes, GetTableName<Node>());
                _logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");

                _logInfoAction($"Started edge import at      : {sw.ElapsedMilliseconds} ms");
                bulky.Commit(modelEdges, GetTableName<Edge>());
                _logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");
              }
              else
              {
                conn.Insert(graph, trans);

                _logInfoAction($"Started node import at      : {sw.ElapsedMilliseconds} ms");
                conn.Insert(modelNodes, trans);
                _logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");

                _logInfoAction($"Started edge import at      : {sw.ElapsedMilliseconds} ms");
                conn.Insert(modelEdges, trans);
                _logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");
              }

              _logInfoAction($"Started database commit     : {sw.ElapsedMilliseconds} ms");
              trans.Commit();
              _logInfoAction($"  finished at               : {sw.ElapsedMilliseconds} ms");

              _logInfoAction(Environment.NewLine);
            }
            _logInfoAction($"Finished!");
            _logInfoAction($"  Imported {modelNodes.Count()} nodes and {modelEdges.Count()} edges in {sw.ElapsedMilliseconds} ms");
          }
        }
      }
    }

    private static void Usage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("  GraphML.Datastore.Database.Importer.CSV.exe [repository-name] [data-file-path.csv]");
      Console.WriteLine();
      Console.WriteLine("Notes:");
      Console.WriteLine("  Database connection is contained in hosting.json");
    }

    private void DumpSettings(IConfiguration config)
    {
      _logInfoAction("Settings:");
      _logInfoAction($"  REPOSITORY:");
      _logInfoAction($"    NAME                        : {_repoName}");
      _logInfoAction($"  DATASTORE:");
      _logInfoAction($"    DATASTORE_CONNECTION        : {Settings.DATASTORE_CONNECTION(config)}");
      _logInfoAction($"    DATASTORE_CONNECTION_TYPE   : {Settings.DATASTORE_CONNECTION_TYPE(config, Settings.DATASTORE_CONNECTION(config))}");
      _logInfoAction($"    DATASTORE_CONNECTION_STRING : {Settings.DATASTORE_CONNECTION_STRING(config, Settings.DATASTORE_CONNECTION(config))}");
    }

    private static string GetTableName<T>()
    {
      var tableName = typeof(T).GetCustomAttribute<Schema.TableAttribute>().Name;

      return tableName;
    }

    private static void LogInformation(string message = null)
    {
      Console.WriteLine(message ?? Environment.NewLine);
    }

    private sealed class ImportEdge
    {
      public string FromNode { get; set; }
      public string ToNode { get; set; }
    }

    private sealed class ImportEdgeClassMap : ClassMap<ImportEdge>
    {
      public ImportEdgeClassMap()
      {
        Map(m => m.FromNode).Index(0);
        Map(m => m.ToNode).Index(1);
      }
    }
  }
}
