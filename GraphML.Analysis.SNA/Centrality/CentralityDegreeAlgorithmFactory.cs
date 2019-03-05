using System;
using QuickGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityDegreeAlgorithmFactory : ICentralityDegreeAlgorithmFactory
  {
    public ICentralityDegreeAlgorithm<string> Create(IEdgeSet<string, IEdge<string>> edgeSet, Func<IEdge<string>, double> weights)
    {
      return new Degree<string, IEdge<string>>(edgeSet, weights);
    }
  }
}
