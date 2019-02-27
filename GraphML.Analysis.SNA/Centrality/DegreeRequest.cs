using System;

namespace GraphML.Analysis.SNA.Centrality
{
  /// <summary>
  /// Request to run SNA 'Degree' on specified graph
  /// </summary>
  public sealed class DegreeRequest : RequestBase
  {
    /// <summary>
    /// Unique identifier of graph
    /// </summary>
    public string GraphId { get; set; }

    public override void Run()
    {
      Console.WriteLine($"DegreeRequest.Run --> {GraphId} @ {CorrelationId}");
    }
  }
}
