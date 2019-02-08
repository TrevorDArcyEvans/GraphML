using CsvHelper.Configuration;
using DapperExtensions;
using GraphML.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            var predicate = Predicates.Field<Repository>(p => p.Name, Operator.Eq, repoName);
            var repo = conn.GetList<Repository>(predicate).Single();
            var graph = new Graph(repo.Id, Path.GetFileNameWithoutExtension(_dataFilePath));

            Console.WriteLine($"Importing from:  {_dataFilePath}");
            Console.WriteLine($"          into:  {conn.ConnectionString}");
            Console.WriteLine($"    repository:  {repo.Name}");
            Console.WriteLine($"         graph:  {graph.Name}");

            var modelNodes = nodes.Select(node => new Node(graph.Id, node)).ToList();
            var modelNodesMap = modelNodes.ToDictionary(node => node.Name);
            var modelEdges = edges.Select(edge =>
              new Edge(
                graph.Id,
                $"{edge.FromNode}-->{edge.ToNode}",
                modelNodesMap[edge.FromNode].Id,
                modelNodesMap[edge.ToNode].Id)).ToList();

            using (var trans = conn.BeginTransaction())
            {
              conn.Insert(graph, trans);
              conn.Insert<Node>(modelNodes, trans);
              conn.Insert<Edge>(modelEdges, trans);

              trans.Commit();
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
