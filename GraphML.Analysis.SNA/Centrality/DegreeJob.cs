using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeJob : JobBase, IDegreeJob
  {
    private readonly IConfiguration _config;
    private readonly IGraphDatastore _graphDatastore;

    public DegreeJob(
      IConfiguration config,
      IGraphDatastore graphDatastore)
    {
      _config = config;
      _graphDatastore = graphDatastore;
    }

    public override void Run(RequestBase req)
    {
      var degReq = (DegreeRequest)req;
      Console.WriteLine($"DegreeJob.Run --> {degReq.GraphId} @ {degReq.CorrelationId}");
    }
  }
}
