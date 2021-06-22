using System;
using System.Collections.Generic;
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
      var nodesMap = nodes.ToDictionary(node => index++, node => node.Id);
      var network = new Network();
      nodesMap
        .Keys
        .ToList()
        .ForEach(id => network.AddVertex(id));

      // raw edges from db
      var edges = _edgeDatastore.ByOwners(new[] { findCommReq.GraphId }, 0, int.MaxValue, null).Items;
      edges.ForEach(edge =>
      {
        var source = nodesMap.Single(x => x.Value == edge.GraphSourceId).Key;
        var target = nodesMap.Single(x => x.Value == edge.GraphTargetId).Key;
        var conn = new Connection(source, target);
        network.AddEdge(conn);
      });

      var algo = new CommunityAlgorithm(network);

      algo.Update();

      var communities = new List<List<Guid>>();
      var allCommNodes = algo.GetCommunityNodes();
      foreach (var commNodes in allCommNodes)
      {
        var community = commNodes.Select(cn => nodesMap[cn.Node]).ToList();
        communities.Add(community);
      }
      var result = new FindCommunitiesResult(communities);
      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
