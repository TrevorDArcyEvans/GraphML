using QuikGraph;
using System;

namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityBetweennessAlgorithmFactory
  {
    ICentralityBetweennessAlgorithm<Guid> Create(IBidirectionalGraph<Guid, IEdge<Guid>> graph, Func<IEdge<Guid>, double> weights);
  }
}
