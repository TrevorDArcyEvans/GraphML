﻿using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeJob : JobBase, IDegreeJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<DegreeJob> _logger;
    private readonly IGraphDatastore _graphDatastore;
    private readonly IResultDatastore _resultDatastore;

    public DegreeJob(
      IConfiguration config,
      ILogger<DegreeJob> logger,
      IGraphDatastore graphDatastore,
      IResultDatastore resultDatastore)
    {
      _config = config;
      _logger = logger;
      _graphDatastore = graphDatastore;
      _resultDatastore = resultDatastore;
    }

    public override void Run(IRequest req)
    {
      var degReq = (DegreeRequest)req;
      _logger.LogInformation($"DegreeJob.Run --> {degReq.GraphId} @ {degReq.CorrelationId}");

      var results = new[]
      {
        new DegreeResult<string>("A",  74,   5),
        new DegreeResult<string>("B",  16,  88),
        new DegreeResult<string>("D",  79,  54),
        new DegreeResult<string>("C",   2,  98),
        new DegreeResult<string>("E",  44, 175),
        new DegreeResult<string>("F", 189,  35),
        new DegreeResult<string>("G",  24,  22),
        new DegreeResult<string>("H",  61,  74),
        new DegreeResult<string>("I",  91,  56),
        new DegreeResult<string>("J",  40,   8),
        new DegreeResult<string>("K",   3,   8)
      };
      var resultJson = JsonConvert.SerializeObject(results);

      _resultDatastore.Create(req, resultJson);
    }
  }
}
