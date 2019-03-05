using System;
using QuickGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityDegreeAlgorithmFactory
  {
      ICentralityDegreeAlgorithm<string> Create(IEdgeSet<string, IEdge<string>> edgeSet, Func<IEdge<string>, double> weights);
  }
}
