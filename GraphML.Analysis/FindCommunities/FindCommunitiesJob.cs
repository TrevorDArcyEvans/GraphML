using System.Linq;
using Comuna;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GraphML.Analysis.FindCommunities
{
  public sealed class FindCommunitiesJob : IFindCommunitiesJob
  {
    private const int MaxCommunities = 10;
    
    private readonly IConfiguration _config;
    private readonly ILogger<FindCommunitiesJob> _logger;
    private readonly IGraphNodeDatastore _nodeDatastore;
    private readonly IGraphEdgeDatastore _edgeDatastore;
    private readonly IResultLogic _resultLogic;

    public FindCommunitiesJob(
      IConfiguration config,
      ILogger<FindCommunitiesJob> logger,
      IGraphNodeDatastore nodeDatastore,
      IGraphEdgeDatastore edgeDatastore,
      IResultLogic resultLogic)
    {
      _config = config;
      _logger = logger;
      _nodeDatastore = nodeDatastore;
      _edgeDatastore = edgeDatastore;
      _resultLogic = resultLogic;
    }

    public void Run(IRequest req)
    {
      var findCommReq = (IFindCommunitiesRequest) req;

      // raw nodes from db
      var nodes = _nodeDatastore.ByOwners(new[] { findCommReq.GraphId }, 0, int.MaxValue, null).Items;
      var index = 0u;
      var indexNodeMap = nodes.ToDictionary(node => index++, node => node.Id);
      var nodeIndexMap = indexNodeMap.ToDictionary(inm => inm.Value, inm => inm.Key);
      var network = new Network();
      indexNodeMap
        .Keys
        .ToList()
        .ForEach(id => network.AddVertex(id));

      // raw edges from db
      var edges = _edgeDatastore.ByOwners(new[] { findCommReq.GraphId }, 0, int.MaxValue, null).Items;
      edges.ForEach(edge =>
      {
        var source = nodeIndexMap[edge.GraphSourceId];
        var target = nodeIndexMap[edge.GraphTargetId];
        var conn = new Connection(source, target);
        network.AddEdge(conn);
      });

      var algo = new CommunityAlgorithm(network);

      algo.Update();

      var allCommNodes = algo.GetCommunityNodes();
      var allCommunities = allCommNodes
        .Select(commNodes => commNodes.Select(cn => indexNodeMap[cn.Node]).ToList())
        .OrderByDescending(x => x.Count)
        .ToList();
      var cutoff = allCommunities.Take(MaxCommunities).Last().Count;
      var communities = allCommunities.Where(x => x.Count >= cutoff).ToList();
      var result = new FindCommunitiesResult(communities);
      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
