using System;
using QuickGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityClosenessAlgorithmFactory : ICentralityClosenessAlgorithmFactory
  {
    public ICentralityClosenessAlgorithm<string> Create(IVertexListGraph<string, IEdge<string>> graph, Func<IEdge<string>, double> weights)
    {
      return new Closeness<string, IEdge<string>>(graph, weights);
    }
  }
}
