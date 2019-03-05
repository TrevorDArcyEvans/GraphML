﻿using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuickGraph;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeJob : JobBase, IDegreeJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<DegreeJob> _logger;
    private readonly IEdgeDatastore _edgeDatastore;
    private readonly ICentralityDegreeAlgorithmFactory _factory;
    private readonly IResultLogic _resultLogic;

    public DegreeJob(
      IConfiguration config,
      ILogger<DegreeJob> logger,
      IEdgeDatastore edgeDatastore,
      ICentralityDegreeAlgorithmFactory factory,
      IResultLogic resultLogic)
    {
      _config = config;
      _logger = logger;
      _edgeDatastore = edgeDatastore;
      _factory = factory;
      _resultLogic = resultLogic;
    }

    public override void Run(IRequest req)
    {
      var degReq = (IDegreeRequest)req;

      // raw edges from db
      var edges = _edgeDatastore.ByOwners(new[] { degReq.GraphId }, 1, int.MaxValue);

      // convert raw edges to QuickGraph edges
      var qgEdges = edges.Select(e => new Edge<string>(e.SourceId, e.TargetId));

      // add edges to graph
      var graph = new EdgeListGraph<string, IEdge<string>>();
      graph.AddEdgeRange(qgEdges);

      // use unweighted edges
      var algo = _factory.Create(graph, e => 1.0);

      var resultsList = new List<DegreeVertexResult<string>>();
      algo.VertexResult += vertexRes => resultsList.Add(vertexRes);

      algo.Compute();

      var result = new DegreeResult<string>(resultsList);
      var resultJson = JsonConvert.SerializeObject(result);

      _resultLogic.Create(req, resultJson);
    }
  }
}
