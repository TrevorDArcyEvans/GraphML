using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuikGraph;
using System;
using System.Linq;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsJob : JobBase, IFindShortestPathsJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<FindShortestPathsJob> _logger;
    private readonly IGraphEdgeDatastore _edgeDatastore;
    private readonly IGraphNodeDatastore _nodeDatastore;
    private readonly IFindShortestPathsAlgorithmFactory _factory;
    private readonly IResultLogic _resultLogic;

    public FindShortestPathsJob(
      IConfiguration config,
      ILogger<FindShortestPathsJob> logger,
      IGraphEdgeDatastore edgeDatastore,
      IGraphNodeDatastore nodeDatastore,
      IFindShortestPathsAlgorithmFactory factory,
      IResultLogic resultLogic)
    {
      _config = config;
      _logger = logger;
      _edgeDatastore = edgeDatastore;
      _nodeDatastore = nodeDatastore;
      _factory = factory;
      _resultLogic = resultLogic;
    }
    public override void Run(IRequest req)
    {
      var shortPathReq = (IFindShortestPathsRequest)req;

      var graph = new BidirectionalGraph<Guid, IEdge<Guid>>();

      var rootNode = _nodeDatastore.ByIds(new[] { shortPathReq.RootNodeId }).Single();
      var graphId = rootNode.OwnerId;

      // raw nodes from db
      var nodes = _nodeDatastore.ByOwners(new[] { graphId }, 0, int.MaxValue, null);

      // convert raw nodes to QuikGraph nodes
      var qgNodes = nodes.Items.Select(n => n.Id);

      // add nodes to graph
      graph.AddVertexRange(qgNodes);

      // raw edges from db
      var edges = _edgeDatastore.ByOwners(new[] { graphId }, 0, int.MaxValue, null);

      // convert raw edges to QuikGraph edges
      // NOTE:  we also create reverse edges
      var qgEdges = edges.Items.Select(e => new Edge<Guid>(e.GraphSourceId, e.GraphTargetId));
      var qgRevEdges = edges.Items.Select(e => new Edge<Guid>(e.GraphTargetId, e.GraphSourceId));

      // add edges + reverse edges to graph
      graph.AddEdgeRange(qgEdges);
      graph.AddEdgeRange(qgRevEdges);

      // use unweighted edges
      var algo = _factory.Create(graph, e => 1.0);

      var result = algo.Compute(shortPathReq.RootNodeId, shortPathReq.GoalNodeId);

      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
