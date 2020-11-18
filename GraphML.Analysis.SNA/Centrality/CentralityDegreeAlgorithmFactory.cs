using System;
using QuickGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityDegreeAlgorithmFactory : ICentralityDegreeAlgorithmFactory
  {
    public ICentralityDegreeAlgorithm<Guid> Create(IEdgeSet<Guid, IEdge<Guid>> edgeSet, Func<IEdge<Guid>, double> weights)
    {
      return new Degree<Guid, IEdge<Guid>>(edgeSet, weights);
    }
  }
}
