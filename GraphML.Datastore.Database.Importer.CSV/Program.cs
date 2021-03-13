using CsvHelper;
using CsvHelper.Configuration;
using Dapper.Contrib.Extensions;
using GraphML.Common;
using GraphML.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Schema = System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Globalization;
using Dapper;
using Newtonsoft.Json;

namespace GraphML.Datastore.Database.Importer.CSV
{
  public sealed class Program
  {
    public static void Main(string[] args)
    {
      // check import spec file
      if (args.Length == 1 && !File.Exists(args[0]))
      {
        Usage();
        return;
      }

      var importSpecPath = args.Length == 0 ? "import.json" : args[0];
      new Program(importSpecPath, LogInformation).Run();
    }

    private readonly ImportSpecification _importSpec = new ImportSpecification();
    private readonly IConfiguration _config;
    private readonly Action<string> _logInfoAction;

    public Program(string importSpecPath, Action<string> logInfoAction)
    {
      _config = new ConfigurationBuilder()
        .AddJsonFile(importSpecPath)
        .Build();
      _config.GetSection("ImportSpecification").Bind(_importSpec);
      _logInfoAction = logInfoAction;
    }

    public void Run()
    {
      SqlMapper.AddTypeHandler(new GuidTypeHandler());
      SqlMapper.RemoveTypeMap(typeof(Guid));
      SqlMapper.RemoveTypeMap(typeof(Guid?));

      DumpSettings(_config, _importSpec);

      var sw = Stopwatch.StartNew();
      var dbConnFact = new DbConnectionFactory(_config);
      using var conn = dbConnFact.Get();
      using var trans = conn.BeginTransaction();
      var org = conn.GetAll<Organisation>().Single(o => o.Name == _importSpec.Organisation);
      var repoMgr = conn.GetAll<RepositoryManager>().Single(rm => rm.Name == _importSpec.RepositoryManager && rm.OrganisationId == org.Id);

      var repo = conn.GetAll<Repository>().SingleOrDefault(r => r.Name == _importSpec.Repository && r.RepositoryManagerId == repoMgr.Id);
      if (repo is null)
      {
        repo = new Repository
        {
          Name = _importSpec.Repository,
          OrganisationId = org.Id,
          RepositoryManagerId = repoMgr.Id
        };
        conn.Insert(repo, trans);
      }

      var edgeAttrDefsMap = new Dictionary<EdgeItemAttributeImportDefinition, EdgeItemAttributeDefinition>();
      foreach (var def in _importSpec.EdgeItemAttributeImportDefinitions)
      {
        var repoDef = conn.GetAll<EdgeItemAttributeDefinition>().SingleOrDefault(x => x.Name == def.Name && x.RepositoryManagerId == repoMgr.Id);
        if (repoDef is null)
        {
          repoDef = new EdgeItemAttributeDefinition
          {
            Name = def.Name,
            DataType = def.DataType,
            OrganisationId = org.Id,
            RepositoryManagerId = repoMgr.Id
          };
          conn.Insert(repoDef, trans);
        }

        edgeAttrDefsMap.Add(def, repoDef);
      }

      var nodeAttrDefsMap = new Dictionary<NodeItemAttributeImportDefinition, NodeItemAttributeDefinition>();
      foreach (var def in _importSpec.NodeItemAttributeImportDefinitions)
      {
        var repoDef = conn.GetAll<NodeItemAttributeDefinition>().SingleOrDefault(x => x.Name == def.Name && x.RepositoryManagerId == repoMgr.Id);
        if (repoDef is null)
        {
          repoDef = new NodeItemAttributeDefinition
          {
            Name = def.Name,
            DataType = def.DataType,
            OrganisationId = org.Id,
            RepositoryManagerId = repoMgr.Id
          };
          conn.Insert(repoDef, trans);
        }

        nodeAttrDefsMap.Add(def, repoDef);
      }

      using var tr = File.OpenText(_importSpec.DataFile);
      var csvCfg = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        Delimiter = Path.GetExtension(_importSpec.DataFile).ToLowerInvariant() == ".csv" ? "," : "\t",
        AllowComments = true,
        HeaderValidated = null,
        HasHeaderRecord = _importSpec.HasHeaderRecord
      };
      using var csv = new CsvHelper.CsvReader(tr, csvCfg);
      if (_importSpec.HasHeaderRecord)
      {
        csv.Read();
        csv.ReadHeader();
      }

      var nodeMap = new Dictionary<string, Node>();
      var edges = new List<Edge>();
      var edgeAttrs = new List<EdgeItemAttribute>();
      var nodeAttrs = new List<NodeItemAttribute>();
      while (csv.Read())
      {
        var srcNodeName = csv[_importSpec.SourceNodeColumn];
        if (!nodeMap.TryGetValue(srcNodeName, out var srcNode))
        {
          srcNode = new Node
          {
            Name = srcNodeName,
            OrganisationId = org.Id,
            RepositoryId = repo.Id
          };
          nodeMap.Add(srcNode.Name, srcNode);
        }

        var tarNodeName = csv[_importSpec.TargetNodeColumn];
        if (!nodeMap.TryGetValue(tarNodeName, out var tarNode))
        {
          tarNode = new Node
          {
            Name = tarNodeName,
            OrganisationId = org.Id,
            RepositoryId = repo.Id
          };
          nodeMap.Add(tarNode.Name, tarNode);
        }

        var edge = new Edge
        {
          SourceId = srcNode.Id,
          TargetId = tarNode.Id,
          Name = $"{srcNode.Name}-->{tarNode.Name}",
          OrganisationId = org.Id,
          RepositoryId = repo.Id
        };
        edges.Add(edge);

        foreach (var kvp in nodeAttrDefsMap)
        {
          var valStr = GetJson(csv, kvp.Key.Columns, kvp.Value.DataType, kvp.Key.DateTimeFormat);
          switch (kvp.Key.ApplyTo)
          {
            case ApplyTo.SourceNode:
              {
                var nodeAttr = new NodeItemAttribute
                {
                  Name = kvp.Value.Name,
                  DataValueAsString = valStr,
                  DefinitionId = kvp.Value.Id,
                  NodeId = srcNode.Id,
                  OrganisationId = org.Id,
                };

                nodeAttrs.Add(nodeAttr);
                break;
              }

            case ApplyTo.TargetNode:
              {
                var nodeAttr = new NodeItemAttribute
                {
                  Name = kvp.Value.Name,
                  DataValueAsString = valStr,
                  DefinitionId = kvp.Value.Id,
                  NodeId = tarNode.Id,
                  OrganisationId = org.Id,
                };

                nodeAttrs.Add(nodeAttr);
                break;
              }

            case ApplyTo.BothNodes:
              {
                var srcNodeAttr = new NodeItemAttribute
                {
                  Name = kvp.Value.Name,
                  DataValueAsString = valStr,
                  DefinitionId = kvp.Value.Id,
                  NodeId = srcNode.Id,
                  OrganisationId = org.Id,
                };
                var tarNodeAttr = new NodeItemAttribute
                {
                  Name = kvp.Value.Name,
                  DataValueAsString = valStr,
                  DefinitionId = kvp.Value.Id,
                  NodeId = tarNode.Id,
                  OrganisationId = org.Id,
                };

                nodeAttrs.AddRange(new[] { srcNodeAttr, tarNodeAttr });
                break;
              }

            default:
              throw new ArgumentOutOfRangeException($"Unknown option:  {kvp.Key.ApplyTo}");
          }
        }

        foreach (var kvp in edgeAttrDefsMap)
        {
          var valStr = GetJson(csv, kvp.Key.Columns, kvp.Value.DataType, kvp.Key.DateTimeFormat);
          var edgeAttr = new EdgeItemAttribute
          {
            Name = kvp.Value.Name,
            DataValueAsString = valStr,
            DefinitionId = kvp.Value.Id,
            EdgeId = edge.Id,
            OrganisationId = org.Id,
          };

          edgeAttrs.Add(edgeAttr);
        }
      }

      var nodes = nodeMap.Values.ToList();

      _logInfoAction($"Transformed data at         : {sw.ElapsedMilliseconds} ms");

      if (conn is SqlConnection sqlConn && trans is SqlTransaction sqlTrans)
      {
        var bulky = new BulkUploadToMsSqlServer(sqlConn, sqlTrans);

        _logInfoAction($"Started node import at            : {sw.ElapsedMilliseconds} ms");
        bulky.Commit(nodes, GetTableName<Node>());
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started edge import at            : {sw.ElapsedMilliseconds} ms");
        bulky.Commit(edges, GetTableName<Edge>());
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started node attribute import at  : {sw.ElapsedMilliseconds} ms");
        bulky.Commit(nodeAttrs, GetTableName<NodeItemAttribute>());
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started edge attribute import at  : {sw.ElapsedMilliseconds} ms");
        bulky.Commit(edgeAttrs, GetTableName<EdgeItemAttribute>());
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");
      }
      else
      {
        _logInfoAction($"Started node import at            : {sw.ElapsedMilliseconds} ms");
        conn.Insert(nodes, trans);
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started edge import at            : {sw.ElapsedMilliseconds} ms");
        conn.Insert(edges, trans);
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started node attribute import at  : {sw.ElapsedMilliseconds} ms");
        conn.Insert(nodeAttrs, trans);
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started edge attribute import at  : {sw.ElapsedMilliseconds} ms");
        conn.Insert(edgeAttrs, trans);
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");
      }

      _logInfoAction($"Started database commit             : {sw.ElapsedMilliseconds} ms");
      trans.Commit();
      _logInfoAction($"  finished at                       : {sw.ElapsedMilliseconds} ms");

      _logInfoAction(Environment.NewLine);
      _logInfoAction($"Finished!");
      _logInfoAction($"  Imported {nodes.Count()} nodes and {edges.Count()} edges in {sw.ElapsedMilliseconds} ms");
    }

    private static void Usage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("  GraphML.Datastore.Database.Importer.CSV.exe <import.json>");
      Console.WriteLine();
      Console.WriteLine("Notes:");
      Console.WriteLine("  Database connection is contained in import.json");
    }

    private void DumpSettings(IConfiguration config, ImportSpecification importSpec)
    {
      _logInfoAction("Settings:");
      _logInfoAction($"  DATA_FILE:");
      _logInfoAction($"    PATH                        : {importSpec.DataFile}");
      _logInfoAction($"  REPOSITORY:");
      _logInfoAction($"    NAME                        : {importSpec.Repository}");
      _logInfoAction($"  DATASTORE:");
      _logInfoAction($"    DATASTORE_CONNECTION        : {config.DATASTORE_CONNECTION()}");
      _logInfoAction($"    DATASTORE_CONNECTION_TYPE   : {config.DATASTORE_CONNECTION_TYPE(config.DATASTORE_CONNECTION())}");
      _logInfoAction($"    DATASTORE_CONNECTION_STRING : {config.DATASTORE_CONNECTION_STRING(config.DATASTORE_CONNECTION())}");
      _logInfoAction(Environment.NewLine);
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

    private static string GetJson(IReaderRow reader, int[] cols, string dataType, string dateTimeFormat)
    {
      var raw = reader[cols[0]];
      switch (dataType)
      {
        case "string":
          {
            var data = raw;
            return JsonConvert.SerializeObject(data);
          }

        case "bool":
          {
            var data = bool.Parse(raw);
            return JsonConvert.SerializeObject(data);
          }

        case "int":
          {
            var data = int.Parse(raw);
            return JsonConvert.SerializeObject(data);
          }

        case "double":
          {
            var data = double.Parse(raw);
            return JsonConvert.SerializeObject(data);
          }

        case "DateTime":
          {
            if (dateTimeFormat == "SecondsSinceUnixEpoch")
            {
              var secs = double.Parse(raw);
              var dt = ConvertFromUnixTimestamp(secs);
              return JsonConvert.SerializeObject(dt);
            }
            var data = dateTimeFormat is null ? DateTime.Parse(raw, CultureInfo.InvariantCulture) : DateTime.ParseExact(raw, dateTimeFormat, CultureInfo.InvariantCulture);
            return JsonConvert.SerializeObject(data);
          }

        case "DateTimeInterval":
          {
            var data = DateTimeInterval.Parse(raw);
            return JsonConvert.SerializeObject(data);
          }

        default:
          throw new ArgumentOutOfRangeException($"Unknown DataType:  {dataType}");
      }
    }

    private static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
      return DateTime.UnixEpoch.AddSeconds(timestamp);
    }
  }
}
