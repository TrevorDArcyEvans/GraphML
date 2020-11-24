using GraphML.Interfaces;

namespace GraphML.Analysis.SNA.Centrality
{
  /// <summary>
  /// Request to run SNA 'Betweenness' on specified graph
  /// </summary>
  public sealed class BetweennessRequest : RequestBase, IBetweennessRequest
  {
    public override string JobType => typeof(IBetweennessJob).AssemblyQualifiedName;
  }
}
