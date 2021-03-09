using System;
using QuikGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityDegreeAlgorithmFactory
  {
      ICentralityDegreeAlgorithm<Guid> Create(IEdgeSet<Guid, IEdge<Guid>> edgeSet, Func<IEdge<Guid>, double> weights);
  }
}
