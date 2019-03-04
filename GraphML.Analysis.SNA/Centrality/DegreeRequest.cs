using GraphML.Interfaces;

namespace GraphML.Analysis.SNA.Centrality
{
  /// <summary>
  /// Request to run SNA 'Degree' on specified graph
  /// </summary>
  public sealed class DegreeRequest : RequestBase, IDegreeRequest
  {
    /// <summary>
    /// Unique identifier of graph
    /// </summary>
    public string GraphId { get; set; }

    public override string JobType => typeof(IDegreeJob).AssemblyQualifiedName;
  }
}
