using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphEdgeDatastore : GraphItemDatastore<GraphEdge>, IGraphEdgeDatastore
  {
    private readonly IGraphDatastore _graphDatastore;
    private readonly IGraphNodeDatastore _graphNodeDatastore;
    private readonly INodeDatastore _nodeDatastore;
    private readonly IEdgeDatastore _edgeDatastore;

    public GraphEdgeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphEdgeDatastore> logger,
      ISyncPolicyFactory policy,
      IGraphDatastore graphDatastore,
      IGraphNodeDatastore graphNodeDatastore,
      INodeDatastore nodeDatastore,
      IEdgeDatastore edgeDatastore) :
      base(dbConnectionFactory, logger, policy)
    {
      _graphDatastore = graphDatastore;
      _graphNodeDatastore = graphNodeDatastore;
      _nodeDatastore = nodeDatastore;
      _edgeDatastore = edgeDatastore;
    }

    public PagedDataEx<GraphEdge> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm)
    {
      if (pageIndex < 0)
      {
        throw new ArgumentOutOfRangeException($"{nameof(pageIndex)} starts at 0");
      }

      return GetInternal(() =>
      {
        var where = $"where ( {nameof(GraphEdge.GraphSourceId)} in ({GetListIds(ids)}) or {nameof(GraphEdge.GraphTargetId)} in ({GetListIds(ids)}) ) and {nameof(GraphEdge.Name)} like '%{searchTerm ?? string.Empty}%'";
        var sql =
          @$"select 
  * from {GetTableName()},
  (select count(*) as {nameof(PagedDataEx<GraphEdge>.TotalCount)} from {GetTableName()} {where} )
{where}
order by {nameof(GraphEdge.Name)} 
{AppendForFetch(pageIndex, pageSize)}";

        var retval = new PagedDataEx<GraphEdge>();
        var items = _dbConnection.Query<GraphEdge, long, GraphEdge>(sql,
          (item, num) =>
          {
            retval.TotalCount = num;
            retval.Items.Add(item);
            return item;
          },
          splitOn: $"{nameof(PagedDataEx<GraphEdge>.TotalCount)}");

        return retval;
      });
    }

    public IEnumerable<GraphEdge> AddByFilter(Guid graphId, string filter)
    {
      var graph = _graphDatastore.ByIds(new[] { graphId }).Single();

      // create missing GraphNodes
      var edges = _edgeDatastore.ByOwners(new[] { graph.RepositoryId }, 0, int.MaxValue, filter).Items;
      var nodeIds = edges
        .SelectMany(e => new[] { e.SourceId, e.TargetId })
        .Distinct()
        .ToList();
      var graphNodes = _graphNodeDatastore.ByOwners(new[] { graphId }, 0, int.MaxValue, null).Items;
      var graphNodeRepoIds = graphNodes.Select(gn => gn.RepositoryItemId);
      var missingGraphNodeRepoIds = nodeIds.Except(graphNodeRepoIds);
      var missingGraphNodeRepo = _nodeDatastore.ByIds(missingGraphNodeRepoIds);
      var missingGraphNodes = missingGraphNodeRepo
        .Select(n => new GraphNode(graphId, graph.OrganisationId, n.Id, n.Name))
        .ToList();
      _graphNodeDatastore.Create(missingGraphNodes);
      graphNodes.AddRange(missingGraphNodes);

      // create new GraphEdges
      var graphEdges = edges
        .Select(e =>
        {
          var source = graphNodes.Single(gn => gn.RepositoryItemId == e.SourceId);
          var target = graphNodes.Single(gn => gn.RepositoryItemId == e.TargetId);
          return new GraphEdge(
            graphId,
            graph.OrganisationId,
            e.Id,
            e.Name,
            source.Id,
            target.Id);
        })
        .ToList();
      Create(graphEdges);

      return graphEdges;
    }
  }
}
