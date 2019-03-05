using System;
using QuickGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityClosenessAlgorithmFactory
  {
      ICentralityClosenessAlgorithm<string> Create(IVertexListGraph<string, IEdge<string>> graph, Func<IEdge<string>, double> weights);
  }
}
