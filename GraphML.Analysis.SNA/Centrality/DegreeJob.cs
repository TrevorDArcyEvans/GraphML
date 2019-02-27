using System;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeJob : JobBase
  {
    public override void Run(RequestBase req)
    {
      var degReq = (DegreeRequest)req;
      Console.WriteLine($"DegreeJob.Run --> {degReq.GraphId} @ {degReq.CorrelationId}");
    }
  }
}
