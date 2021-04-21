﻿using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuikGraph;
using System;
using System.Linq;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class BetweennessJob : JobBase, IBetweennessJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<BetweennessJob> _logger;
    private readonly IEdgeDatastore _edgeDatastore;
    private readonly INodeDatastore _nodeDatastore;
    private readonly ICentralityBetweennessAlgorithmFactory _factory;
    private readonly IResultLogic _resultLogic;

    public BetweennessJob(
      IConfiguration config,
      ILogger<BetweennessJob> logger,
      IEdgeDatastore edgeDatastore,
      INodeDatastore nodeDatastore,
      ICentralityBetweennessAlgorithmFactory factory,
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
      var closeReq = (IBetweennessRequest)req;

      var graph = new BidirectionalGraph<Guid, IEdge<Guid>>();

      // raw nodes from db
      var nodes = _nodeDatastore.ByOwners(new[] { closeReq.GraphId }, 1, int.MaxValue);

      // convert raw nodes to QuikGraph nodes
      var qgNodes = nodes.Items.Select(n => n.Id);

      // add nodes to graph
      graph.AddVertexRange(qgNodes);

      // raw edges from db
      var edges = _edgeDatastore.ByOwners(new[] { closeReq.GraphId }, 1, int.MaxValue);

      // convert raw edges to QuikGraph edges
      // NOTE:  we also create reverse edges
      var qgEdges = edges.Items.Select(e => new Edge<Guid>(e.SourceId, e.TargetId));
      var qgRevEdges = edges.Items.Select(e => new Edge<Guid>(e.TargetId, e.SourceId));

      // add edges + reverse edges to graph
      graph.AddEdgeRange(qgEdges);
      graph.AddEdgeRange(qgRevEdges);

      // use unweighted edges
      var algo = _factory.Create(graph, e => 1.0);

      var result = algo.Compute();

      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
