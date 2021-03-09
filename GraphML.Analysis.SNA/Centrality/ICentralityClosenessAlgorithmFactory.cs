using System;
using QuikGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityClosenessAlgorithmFactory
  {
      ICentralityClosenessAlgorithm<Guid> Create(IVertexListGraph<Guid, IEdge<Guid>> graph, Func<IEdge<Guid>, double> weights);
  }
}
