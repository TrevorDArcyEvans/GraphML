using GraphML.Interfaces;

namespace GraphML.Analysis.SNA.Centrality
{
  /// <summary>
  /// Request to run SNA 'Closeness' on specified graph
  /// </summary>
  public sealed class ClosenessRequest : RequestBase, IClosenessRequest
  {
    /// <summary>
    /// Unique identifier of graph
    /// </summary>
    public string GraphId { get; set; }

    public override string JobType => typeof(IClosenessJob).AssemblyQualifiedName;
  }
}
