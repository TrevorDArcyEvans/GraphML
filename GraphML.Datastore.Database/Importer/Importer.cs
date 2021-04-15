﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Dapper.Contrib.Extensions;
using GraphML.Common;
using GraphML.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML.Datastore.Database.Importer
{
  public sealed class Importer
  {
    private readonly ImportSpecification _importSpec;
    private readonly IConfiguration _config;
    private readonly Stream _stream;
    private readonly Action<string> _logInfoAction;

    public Importer(
      ImportSpecification importSpec,
      IConfiguration config,
      Stream stream,
      Action<string> logInfoAction = null)
    {
      _importSpec = importSpec;
      _config = config;
      _stream = stream;
      _logInfoAction = logInfoAction ?? (message => Console.WriteLine(message ?? Environment.NewLine));
    }

    public void Run()
    {
      SqlMapper.AddTypeHandler(new GuidTypeHandler());
      SqlMapper.RemoveTypeMap(typeof(Guid));
      SqlMapper.RemoveTypeMap(typeof(Guid?));

      DumpSettings(_config, _importSpec);

      var sw = Stopwatch.StartNew();
      _logInfoAction($"Started at                        : {sw.ElapsedMilliseconds} ms");
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

      var edgeAttrDefsMap = GetEdgeItemAttributeDefinitionsMap(conn, trans, repoMgr, org);
      var nodeAttrDefsMap = GetNodeItemAttributeDefinitionsMap(conn, trans, repoMgr, org);

      _logInfoAction($"Started file read                 : {sw.ElapsedMilliseconds} ms");
      using var tr = new StreamReader(_stream);
      var csvCfg = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        Delimiter = Path.GetExtension(_importSpec.DataFile).ToLowerInvariant() == ".csv" ? "," : "\t",
        AllowComments = true,
        HeaderValidated = null,
        HasHeaderRecord = _importSpec.HasHeaderRecord,
        MissingFieldFound = null
      };
      using var csv = new CsvReader(tr, csvCfg);
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
        var srcNode = GetOrCreateNode(csv, _importSpec.SourceNodeColumn, org, repo, nodeMap);
        var tarNode = GetOrCreateNode(csv, _importSpec.TargetNodeColumn, org, repo, nodeMap);
        if (!(srcNode is null) && !(tarNode is null))
        {
          var edge = new Edge
          {
            SourceId = srcNode.Id,
            TargetId = tarNode.Id,
            Name = $"{srcNode.Name}-->{tarNode.Name}",
            OrganisationId = org.Id,
            RepositoryId = repo.Id
          };
          edges.Add(edge);
          ProcessEdgeItemAttributes(edgeAttrDefsMap, csv, edge, org, edgeAttrs);
        }

        ProcessNodeItemAttributes(nodeAttrDefsMap, csv, srcNode, tarNode, org, nodeAttrs);
      }

      _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

      var nodes = nodeMap.Values.ToList();

      _logInfoAction($"Transformed data at               : {sw.ElapsedMilliseconds} ms");

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

      _logInfoAction($"Started database commit           : {sw.ElapsedMilliseconds} ms");
      trans.Commit();
      _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

      _logInfoAction(Environment.NewLine);
      _logInfoAction($"Finished!");
      _logInfoAction(Environment.NewLine);
      _logInfoAction($"Summary:");
      _logInfoAction($"  Nodes          : {nodes.Count()}");
      _logInfoAction($"    Attributes   : {nodeAttrs.Count()}");
      _logInfoAction($"  Edges          : {edges.Count()}");
      _logInfoAction($"    Attributes   : {edgeAttrs.Count()}");
      _logInfoAction($"  Elapsed time   : {sw.ElapsedMilliseconds} ms");
    }

    private Node GetOrCreateNode(
      CsvReader csv,
      int importSpecNodeColumn,
      Organisation org,
      Repository repo,
      Dictionary<string, Node> nodeMap)
    {
      var nodeName = csv[importSpecNodeColumn];
      if (nodeName is null)
      {
        return null;
      }

      if (nodeMap.TryGetValue(nodeName, out var node))
      {
        return node;
      }

      node = new Node
      {
        Name = nodeName,
        OrganisationId = org.Id,
        RepositoryId = repo.Id
      };
      nodeMap.Add(node.Name, node);

      return node;
    }

    private static void ProcessNodeItemAttributes(
      Dictionary<NodeItemAttributeImportDefinition, NodeItemAttributeDefinition> nodeAttrDefsMap,
      CsvReader csv,
      Node srcNode, Node tarNode,
      Organisation org,
      List<NodeItemAttribute> nodeAttrs)
    {
      foreach (var kvp in nodeAttrDefsMap)
      {
        var valStr = GetJson(csv, kvp.Key.Columns, kvp.Value.DataType, kvp.Key.DateTimeFormat);
        if (valStr is null)
        {
          continue;
        }

        switch (kvp.Key.ApplyTo)
        {
          case ApplyTo.SourceNode:
          {
            if (srcNode is not null)
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
            }

            break;
          }

          case ApplyTo.TargetNode:
          {
            if (tarNode is not null)
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
            }

            break;
          }

          case ApplyTo.BothNodes:
          {
            if (srcNode is not null)
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
            }

            if (tarNode is not null)
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
            }

            break;
          }

          default:
            throw new ArgumentOutOfRangeException($"Unknown option:  {kvp.Key.ApplyTo}");
        }
      }
    }

    private static void ProcessEdgeItemAttributes(
      Dictionary<EdgeItemAttributeImportDefinition, EdgeItemAttributeDefinition> edgeAttrDefsMap,
      CsvReader csv,
      Edge edge,
      Organisation org,
      List<EdgeItemAttribute> edgeAttrs)
    {
      foreach (var kvp in edgeAttrDefsMap)
      {
        var valStr = GetJson(csv, kvp.Key.Columns, kvp.Value.DataType, kvp.Key.DateTimeFormat);
        if (valStr is null)
        {
          continue;
        }

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

    private Dictionary<EdgeItemAttributeImportDefinition, EdgeItemAttributeDefinition> GetEdgeItemAttributeDefinitionsMap(
      IDbConnection conn,
      IDbTransaction trans,
      RepositoryManager repoMgr,
      Organisation org)
    {
      var edgeAttrDefsMap = new Dictionary<EdgeItemAttributeImportDefinition, EdgeItemAttributeDefinition>();
      foreach (var def in _importSpec.EdgeItemAttributeImportDefinitions)
      {
        var repoDef = conn.GetAll<EdgeItemAttributeDefinition>()
          .SingleOrDefault(x => x.Name == def.Name && x.RepositoryManagerId == repoMgr.Id);
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

      return edgeAttrDefsMap;
    }

    private Dictionary<NodeItemAttributeImportDefinition, NodeItemAttributeDefinition> GetNodeItemAttributeDefinitionsMap(
      IDbConnection conn,
      IDbTransaction trans,
      RepositoryManager
        repoMgr, Organisation org)
    {
      var nodeAttrDefsMap = new Dictionary<NodeItemAttributeImportDefinition, NodeItemAttributeDefinition>();
      foreach (var def in _importSpec.NodeItemAttributeImportDefinitions)
      {
        var repoDef = conn.GetAll<NodeItemAttributeDefinition>()
          .SingleOrDefault(x => x.Name == def.Name && x.RepositoryManagerId == repoMgr.Id);
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

      return nodeAttrDefsMap;
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

    private static string GetJson(IReaderRow reader, int[] cols, string dataType, string dateTimeFormat)
    {
      switch (dataType)
      {
        case "string":
        {
          var raw = reader[cols[0]];
          if (raw is null)
          {
            return null;
          }

          var data = raw;
          return JsonConvert.SerializeObject(data);
        }

        case "bool":
        {
          var raw = reader[cols[0]];
          if (raw is null)
          {
            return null;
          }

          var data = bool.Parse(raw);
          return JsonConvert.SerializeObject(data);
        }

        case "int":
        {
          var raw = reader[cols[0]];
          if (raw is null)
          {
            return null;
          }

          var data = int.Parse(raw);
          return JsonConvert.SerializeObject(data);
        }

        case "double":
        {
          var raw = reader[cols[0]];
          if (raw is null)
          {
            return null;
          }

          var data = double.Parse(raw);
          return JsonConvert.SerializeObject(data);
        }

        case "DateTime":
        {
          var raw = reader[cols[0]];
          if (raw is null)
          {
            return null;
          }

          var data = ParseDateTime(raw, dateTimeFormat);
          return JsonConvert.SerializeObject(data);
        }

        case "DateTimeInterval":
        {
          var startStr = reader[cols[0]];
          var endStr = reader[cols[1]];
          if (startStr is null || endStr is null)
          {
            return null;
          }

          var start = ParseDateTime(startStr, dateTimeFormat);
          var end = ParseDateTime(endStr, dateTimeFormat);
          var data = new DateTimeInterval(start, end);
          return JsonConvert.SerializeObject(data);
        }

        default:
          throw new ArgumentOutOfRangeException($"Unknown DataType:  {dataType}");
      }
    }

    private static DateTime ParseDateTime(string raw, string dateTimeFormat)
    {
      if (dateTimeFormat == "SecondsSinceUnixEpoch")
      {
        var secs = double.Parse(raw);
        var dt = ConvertFromUnixTimestamp(secs);
        return dt;
      }

      var data = dateTimeFormat is null ? DateTime.Parse(raw, CultureInfo.InvariantCulture) : DateTime.ParseExact(raw, dateTimeFormat, CultureInfo.InvariantCulture);
      return data;
    }

    private static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
      return DateTime.UnixEpoch.AddSeconds(timestamp);
    }
  }
}