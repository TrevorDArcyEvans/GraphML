using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeJob : JobBase, IDegreeJob
  {
    private readonly IConfiguration _config;
    private readonly ILogger<DegreeJob> _logger;
    private readonly IGraphDatastore _graphDatastore;

    public DegreeJob(
      IConfiguration config,
      ILogger<DegreeJob> logger,
      IGraphDatastore graphDatastore)
    {
      _config = config;
      _logger = logger;
      _graphDatastore = graphDatastore;
    }

    public override void Run(RequestBase req)
    {
      var degReq = (DegreeRequest)req;
      Console.WriteLine($"DegreeJob.Run --> {degReq.GraphId} @ {degReq.CorrelationId}");

      // simulates a log running process
      Thread.Sleep(TimeSpan.FromMinutes(10));
    }
  }
}
