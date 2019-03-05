using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuickGraph;
using System.Linq;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsJob : JobBase, IFindShortestPathsJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<FindShortestPathsJob> _logger;
    private readonly IEdgeDatastore _edgeDatastore;
    private readonly INodeDatastore _nodeDatastore;
    private readonly IFindShortestPathsAlgorithmFactory _factory;
    private readonly IResultLogic _resultLogic;

    public FindShortestPathsJob(
      IConfiguration config,
      ILogger<FindShortestPathsJob> logger,
      IEdgeDatastore edgeDatastore,
      INodeDatastore nodeDatastore,
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

      var graph = new BidirectionalGraph<string, IEdge<string>>();

      var rootNode = _nodeDatastore.ByIds(new[] { shortPathReq.RootNodeId }).Single();
      var graphId = rootNode.OwnerId;

      // raw nodes from db
      var nodes = _nodeDatastore.ByOwners(new[] { graphId }, 1, int.MaxValue);

      // convert raw nodes to QuickGraph nodes
      var qgNodes = nodes.Select(n => n.Id);

      // add nodes to graph
      graph.AddVertexRange(qgNodes);

      // raw edges from db
      var edges = _edgeDatastore.ByOwners(new[] { graphId }, 1, int.MaxValue);

      // convert raw edges to QuickGraph edges
      // NOTE:  we also create reverse edges
      var qgEdges = edges.Select(e => new Edge<string>(e.SourceId, e.TargetId));
      var qgRevEdges = edges.Select(e => new Edge<string>(e.TargetId, e.SourceId));

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
