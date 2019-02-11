using CsvHelper.Configuration;
using Dapper.Contrib.Extensions;
using GraphML.Utils;
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
      if (args.Length != 1 || !File.Exists(args[0]))
      {
        Usage();
        return;
      }

      new Program(args[0]).Run();
    }

    private readonly string _dataFilePath;

    public Program(string dataFilePath)
    {
      _dataFilePath = dataFilePath;
    }

    private void Run()
    {
      var config = new ConfigurationBuilder()
        .AddJsonFile("hosting.json")
        .AddEnvironmentVariables()
        .Build();

      DumpSettings(config);
      Console.WriteLine();

      var sw = Stopwatch.StartNew();
      using (var tr = File.OpenText(_dataFilePath))
      {
        var csvCfg = new Configuration
        {
          Delimiter = "\t",
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
            var repoName = GetRepositoryName();
            var repo = conn.GetAll<Repository>().Single(r => r.Name == repoName);
            var graph = new Graph(repo.Id, Path.GetFileNameWithoutExtension(_dataFilePath));

            Console.WriteLine($"Importing from:  {_dataFilePath}");
            Console.WriteLine($"          into:  {conn.ConnectionString}");
            Console.WriteLine($"    repository:  {repo.Name}");
            Console.WriteLine($"         graph:  {graph.Name}");
            Console.WriteLine();

            var modelNodes = nodes.Select(node => new Node(graph.Id, node)).ToList();
            var modelNodesMap = modelNodes.ToDictionary(node => node.Name);
            var modelEdges = edges.Select(edge =>
              new Edge(
                graph.Id,
                $"{edge.FromNode}-->{edge.ToNode}",
                modelNodesMap[edge.FromNode].Id,
                modelNodesMap[edge.ToNode].Id)).ToList();

            Console.WriteLine($"Transformed data at         : {sw.ElapsedMilliseconds} ms");

            using (var trans = conn.BeginTransaction())
            {
              if (conn is SqlConnection && trans is SqlTransaction)
              {
                var bulky = new BulkUploadToMsSqlServer((SqlConnection)conn, (SqlTransaction)trans);

                bulky.Commit(new[] { graph }, GetTableName<Graph>());

                Console.WriteLine($"Started node import at      : {sw.ElapsedMilliseconds} ms");
                bulky.Commit(modelNodes, GetTableName<Node>());
                Console.WriteLine($"  finished at               : {sw.ElapsedMilliseconds} ms");

                Console.WriteLine($"Started edge import at      : {sw.ElapsedMilliseconds} ms");
                bulky.Commit(modelEdges, GetTableName<Edge>());
                Console.WriteLine($"  finished at               : {sw.ElapsedMilliseconds} ms");
              }
              else
              {
                conn.Insert(graph, trans);

                Console.WriteLine($"Started node import at      : {sw.ElapsedMilliseconds} ms");
                conn.Insert(modelNodes, trans);
                Console.WriteLine($"  finished at               : {sw.ElapsedMilliseconds} ms");

                Console.WriteLine($"Started edge import at      : {sw.ElapsedMilliseconds} ms");
                conn.Insert(modelEdges, trans);
                Console.WriteLine($"  finished at               : {sw.ElapsedMilliseconds} ms");
              }

              Console.WriteLine($"Started database commit     : {sw.ElapsedMilliseconds} ms");
              trans.Commit();
              Console.WriteLine($"  finished at               : {sw.ElapsedMilliseconds} ms");

              Console.WriteLine();
            }
            Console.WriteLine($"Finished!");
            Console.WriteLine($"  Imported {modelNodes.Count()} nodes and {modelEdges.Count()} edges in {sw.ElapsedMilliseconds} ms");
          }
        }
      }
    }

    private static void Usage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("  GraphML.Datastore.Database.Importer.CSV.exe [data-file-path.csv]");
      Console.WriteLine();
      Console.WriteLine("Notes:");
      Console.WriteLine("  Database connection is contained in hosting.json");
    }

    private static void DumpSettings(IConfiguration config)
    {
      Console.WriteLine("Settings:");
      Console.WriteLine($"  REPOSITORY:");
      Console.WriteLine($"    NAME                        : {GetRepositoryName()}");
      Console.WriteLine($"  DATASTORE:");
      Console.WriteLine($"    DATASTORE_CONNECTION        : {Settings.DATASTORE_CONNECTION(config)}");
      Console.WriteLine($"    DATASTORE_CONNECTION_TYPE   : {Settings.DATASTORE_CONNECTION_TYPE(config, Settings.DATASTORE_CONNECTION(config))}");
      Console.WriteLine($"    DATASTORE_CONNECTION_STRING : {Settings.DATASTORE_CONNECTION_STRING(config, Settings.DATASTORE_CONNECTION(config))}");
    }

    private static string GetTableName<T>()
    {
      var tableName = typeof(T).GetCustomAttribute<Schema.TableAttribute>().Name;

      return tableName;
    }

    private static string GetRepositoryName()
    {
      return Environment.GetEnvironmentVariable("REPOSITORY_NAME");
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
