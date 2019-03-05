using QuickGraph;
using System;

namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityBetweennessAlgorithmFactory
  {
    ICentralityBetweennessAlgorithm<string> Create(IBidirectionalGraph<string, IEdge<string>> graph, Func<IEdge<string>, double> weights);
  }
}
