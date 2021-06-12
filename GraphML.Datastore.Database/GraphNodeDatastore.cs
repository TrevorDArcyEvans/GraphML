using System;
using System.Collections.Generic;
using System.Linq;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphNodeDatastore : GraphItemDatastore<GraphNode>, IGraphNodeDatastore
  {
    private readonly IGraphDatastore _graphDatastore;
    private readonly INodeDatastore _nodeDatastore;
    
    public GraphNodeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphNodeDatastore> logger,
      ISyncPolicyFactory policy,
      IGraphDatastore graphDatastore,
      INodeDatastore nodeDatastore) :
      base(dbConnectionFactory, logger, policy)
    {
      _graphDatastore = graphDatastore;
      _nodeDatastore = nodeDatastore;
    }

    public IEnumerable<GraphNode> AddByFilter(Guid graphId, string filter)
    {
      var graph = _graphDatastore.ByIds(new[] { graphId }).Single();
      var repoId = graph.RepositoryId;
      var nodes = _nodeDatastore.ByOwners(new[] { repoId }, 0, int.MaxValue, filter).Items;
      var graphNodes = nodes.Select(n => new GraphNode(graphId, graph.OrganisationId, n.Id, n.Name)).ToList();

      Create(graphNodes);

      return graphNodes;
    }
  }
}
