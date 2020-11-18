using System;
using QuickGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityClosenessAlgorithmFactory : ICentralityClosenessAlgorithmFactory
  {
    public ICentralityClosenessAlgorithm<Guid> Create(IVertexListGraph<Guid, IEdge<Guid>> graph, Func<IEdge<Guid>, double> weights)
    {
      return new Closeness<Guid, IEdge<Guid>>(graph, weights);
    }
  }
}
