using QuikGraph;
using System;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityBetweennessAlgorithmFactory : ICentralityBetweennessAlgorithmFactory
  {
    public ICentralityBetweennessAlgorithm<Guid> Create(IBidirectionalGraph<Guid, IEdge<Guid>> graph, Func<IEdge<Guid>, double> weights)
    {
      return new Betweenness<Guid, IEdge<Guid>>(graph, weights);
    }
  }
}
