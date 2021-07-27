using System;
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
      var srcNodeSet = new HashSet<Guid>();
      while (csv.Read())
      {
        var srcNode = GetOrCreateNode(csv, _importSpec.SourceNodeColumn, org, repo, nodeMap);
        var tarNode = GetOrCreateNode(csv, _importSpec.TargetNodeColumn, org, repo, nodeMap);
        if (srcNode is not null && tarNode is not null)
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

        if (srcNode is not null &&
            !srcNodeSet.Contains(srcNode.Id))
        {
          srcNodeSet.Add(srcNode.Id);
        }

        ProcessNodeItemAttributes(nodeAttrDefsMap, csv, srcNode, tarNode, org, nodeAttrs);
      }

      _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

      var nodes = nodeMap.Values.ToList();

      _logInfoAction($"Transformed data at               : {sw.ElapsedMilliseconds} ms");

      var graphChartName = Path.GetFileNameWithoutExtension(_importSpec.DataFile);
      var graph = new Graph(repo.Id, org.Id, graphChartName);
      var graphNodes = nodes.Select(n => new GraphNode(graph.Id, org.Id, n.Id, n.Name)).ToList();
      var graphNodeMap = graphNodes.ToDictionary(gn => gn.RepositoryItemId, gn => gn.Id);
      var graphNodeRevMap = graphNodes.ToDictionary(gn => gn.Id, gn => gn.RepositoryItemId);
      var graphEdges = edges
        .Select(e =>
        {
          var sourceId = graphNodeMap[e.SourceId];
          var targetId = graphNodeMap[e.TargetId];
          return new GraphEdge(
            graph.Id,
            graph.OrganisationId,
            e.Id,
            e.Name,
            sourceId,
            targetId);
        })
        .ToList();
      var chart = new Chart(graph.Id, org.Id, graphChartName);
      var chartNodes = graphNodes
        .Select(gn =>
        {
          var nodeId = graphNodeRevMap[gn.Id];
          var isSource = srcNodeSet.Contains(nodeId);
          return new ChartNode(chart.Id, org.Id, gn.Id, gn.Name)
          {
            IconName = isSource ? _importSpec.SourceIconName : _importSpec.TargetIconName
          };
        })
        .ToList();
      var chartNodesMap = chartNodes.ToDictionary(cn => cn.GraphItemId, cn => cn.Id);
      var chartEdges = graphEdges
        .Select(ge =>
          new ChartEdge(
            chart.Id,
            ge.OrganisationId,
            ge.Id,
            ge.Name,
            chartNodesMap[ge.GraphSourceId],
            chartNodesMap[ge.GraphTargetId]))
        .ToList();

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

        bulky.Commit(new[] { graph }, GetTableName<Graph>());
        _logInfoAction($"Started graph node import at      : {sw.ElapsedMilliseconds} ms");
        bulky.Commit(graphNodes, GetTableName<GraphNode>());
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started graph edge import at      : {sw.ElapsedMilliseconds} ms");
        bulky.Commit(graphEdges, GetTableName<GraphEdge>());
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        bulky.Commit(new[] { chart }, GetTableName<Chart>());
        _logInfoAction($"Started chart node import at      : {sw.ElapsedMilliseconds} ms");
        bulky.Commit(chartNodes, GetTableName<ChartNode>());
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started chart edge import at      : {sw.ElapsedMilliseconds} ms");
        bulky.Commit(chartEdges, GetTableName<ChartEdge>());
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

        conn.Insert(graph, trans);
        _logInfoAction($"Started graph node import at      : {sw.ElapsedMilliseconds} ms");
        conn.Insert(graphNodes, trans);
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started graph edge import at      : {sw.ElapsedMilliseconds} ms");
        conn.Insert(graphEdges, trans);
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        conn.Insert(chart, trans);
        _logInfoAction($"Started chart node import at      : {sw.ElapsedMilliseconds} ms");
        conn.Insert(chartNodes, trans);
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

        _logInfoAction($"Started chart edge import at      : {sw.ElapsedMilliseconds} ms");
        conn.Insert(chartEdges, trans);
        _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");
      }

      _logInfoAction($"Started database commit           : {sw.ElapsedMilliseconds} ms");
      trans.Commit();
      _logInfoAction($"  finished at                     : {sw.ElapsedMilliseconds} ms");

      _logInfoAction(Environment.NewLine);
      _logInfoAction($"Finished!");
      _logInfoAction(Environment.NewLine);
      _logInfoAction($"Summary:");
      _logInfoAction($"  Nodes          : {nodes.Count}");
      _logInfoAction($"    Attributes   : {nodeAttrs.Count}");
      _logInfoAction($"  Edges          : {edges.Count}");
      _logInfoAction($"    Attributes   : {edgeAttrs.Count}");
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
            ApplyToSourceNode(srcNode, org, nodeAttrs, kvp, valStr);
            break;

          case ApplyTo.TargetNode:
            ApplyToTargetNode(tarNode, org, nodeAttrs, kvp, valStr);
            break;

          case ApplyTo.BothNodes:
            ApplyToBothNodes(srcNode, tarNode, org, nodeAttrs, kvp, valStr);
            break;

          default:
            throw new ArgumentOutOfRangeException($"Unknown option:  {kvp.Key.ApplyTo}");
        }
      }
    }

    private static void ApplyToBothNodes(
      Node srcNode, 
      Node tarNode, 
      Organisation org, 
      List<NodeItemAttribute> nodeAttrs, 
      KeyValuePair<NodeItemAttributeImportDefinition, NodeItemAttributeDefinition> kvp, 
      string valStr)
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
    }

    private static void ApplyToTargetNode(
      Node tarNode, 
      Organisation org, 
      List<NodeItemAttribute> nodeAttrs, 
      KeyValuePair<NodeItemAttributeImportDefinition, NodeItemAttributeDefinition> kvp, 
      string valStr)
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
    }

    private static void ApplyToSourceNode(
      Node srcNode, 
      Organisation org, 
      List<NodeItemAttribute> nodeAttrs, 
      KeyValuePair<NodeItemAttributeImportDefinition, NodeItemAttributeDefinition> kvp, 
      string valStr)
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
      RepositoryManager repoMgr, 
      Organisation org)
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
          return GetJsonString(reader, cols);

        case "bool":
          return GetJsonBool(reader, cols);

        case "int":
          return GetJsonInt(reader, cols);

        case "double":
          return GetJsonDouble(reader, cols);

        case "DateTime":
          return GetJsonDateTime(reader, cols, dateTimeFormat);

        case "DateTimeInterval":
          return GetJsonDateTimeInterval(reader, cols, dateTimeFormat);

        default:
          throw new ArgumentOutOfRangeException($"Unknown DataType:  {dataType}");
      }
    }

    private static string GetJsonDateTimeInterval(IReaderRow reader, int[] cols, string dateTimeFormat)
    {
      var startStr = reader[cols[0]];
      var endStr = reader[cols[1]];
      if (startStr is null || endStr is null)
      {
        return null;
      }

      var start = ParseDateTime(startStr, dateTimeFormat);
      var end = ParseDateTime(endStr, dateTimeFormat);
      if (start is null ||
          end is null)
      {
        return null;
      }

      var data = new DateTimeInterval(start.Value, end.Value);
      return JsonConvert.SerializeObject(data);
    }

    private static string GetJsonDateTime(IReaderRow reader, int[] cols, string dateTimeFormat)
    {
      var raw = reader[cols[0]];
      if (raw is null)
      {
        return null;
      }

      var data = ParseDateTime(raw, dateTimeFormat);
      return data is null ? null : JsonConvert.SerializeObject(data.Value);
    }

    private static string GetJsonDouble(IReaderRow reader, int[] cols)
    {
      var raw = reader[cols[0]];
      if (raw is null)
      {
        return null;
      }

      return JsonConvert.SerializeObject(double.TryParse(raw, out var data) ? data : null);
    }

    private static string GetJsonInt(IReaderRow reader, int[] cols)
    {
      var raw = reader[cols[0]];
      if (raw is null)
      {
        return null;
      }

      return JsonConvert.SerializeObject(int.TryParse(raw, out var data) ? data : null);
    }

    private static string GetJsonBool(IReaderRow reader, int[] cols)
    {
      var raw = reader[cols[0]];
      if (raw is null)
      {
        return null;
      }

      return JsonConvert.SerializeObject(bool.TryParse(raw, out var data) ? data : null);
    }

    private static string GetJsonString(IReaderRow reader, int[] cols)
    {
      var raw = reader[cols[0]];
      if (raw is null)
      {
        return null;
      }

      var data = raw;
      return JsonConvert.SerializeObject(data);
    }

    private static DateTime? ParseDateTime(string raw, string dateTimeFormat)
    {
      if (dateTimeFormat == "SecondsSinceUnixEpoch")
      {
        var secs = double.Parse(raw);
        var dt = ConvertFromUnixTimestamp(secs);
        return dt;
      }

      {
        if (dateTimeFormat is null)
        {
          if (DateTime.TryParse(raw, out var data))
          {
            return data;
          }
        }
      }

      {
        if (DateTime.TryParseExact(raw, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
        {
          return data;
        }
      }

      {
        // Last ditch attempt for IMDb-movies.csv which usually puts dates in yyyy-mm-dd but sometimes only has the year
        if (DateTime.TryParseExact(raw, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
        {
          return data;
        }
      }

      return null;
    }

    private static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
      return DateTime.UnixEpoch.AddSeconds(timestamp);
    }
  }
}
