using GraphML.Interfaces;
using System;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeJob : JobBase, IDegreeJob
  {
    private readonly IGraphDatastore _graphDatastore;

    public DegreeJob(IGraphDatastore graphDatastore)
    {
      _graphDatastore = graphDatastore;
    }

    public override void Run(RequestBase req)
    {
      var degReq = (DegreeRequest)req;
      Console.WriteLine($"DegreeJob.Run --> {degReq.GraphId} @ {degReq.CorrelationId}");
    }
  }
}
