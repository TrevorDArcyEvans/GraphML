using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class ClosenessJob : JobBase, IClosenessJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<ClosenessJob> _logger;
    private readonly IEdgeDatastore _edgeDatastore;
    private readonly INodeDatastore _nodeDatastore;
    private readonly ICentralityClosenessAlgorithmFactory _factory;
    private readonly IResultLogic _resultLogic;

    public ClosenessJob(
      IConfiguration config,
      ILogger<ClosenessJob> logger,
      IEdgeDatastore edgeDatastore,
      INodeDatastore nodeDatastore,
      ICentralityClosenessAlgorithmFactory factory,
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
      var closeReq = (IClosenessRequest)req;

      var graph = new AdjacencyGraph<Guid, IEdge<Guid>>();

      // raw nodes from db
      var nodes = _nodeDatastore.ByOwners(new[] { closeReq.GraphId }, 1, int.MaxValue);

      // convert raw nodes to QuikGraph nodes
      var qgNodes = nodes.Items.Select(n => n.Id);

      // add nodes to graph
      graph.AddVertexRange(qgNodes);

      // raw edges from db
      var edges = _edgeDatastore.ByOwners(new[] { closeReq.GraphId }, 1, int.MaxValue);

      // convert raw edges to QuikGraph edges
      var qgEdges = edges.Items.Select(e => new Edge<Guid>(e.SourceId, e.TargetId));

      // add edges to graph
      graph.AddEdgeRange(qgEdges);

      // use unweighted edges
      var algo = _factory.Create(graph, e => 1.0);

      var resultsList = new List<ClosenessVertexResult<Guid>>();
      algo.VertexResult += vertexRes => resultsList.Add(vertexRes);

      algo.Compute();

      var result = new ClosenessResult<Guid>(resultsList);
      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
