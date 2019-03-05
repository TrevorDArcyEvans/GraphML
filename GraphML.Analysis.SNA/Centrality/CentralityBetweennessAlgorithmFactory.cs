using QuickGraph;
using System;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityBetweennessAlgorithmFactory : ICentralityBetweennessAlgorithmFactory
  {
    public ICentralityBetweennessAlgorithm<string> Create(IBidirectionalGraph<string, IEdge<string>> graph, Func<IEdge<string>, double> weights)
    {
      return new Betweenness<string, IEdge<string>>(graph, weights);
    }
  }
}
