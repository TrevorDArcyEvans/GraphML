using System;
using QuickGraph;

namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityDegreeAlgorithm<TVertex>
  {
    event DegreeResultAction<TVertex> VertexResult;
    void Compute();
  }

  public interface ICentralityDegreeAlgorithmFactory
  {
      ICentralityDegreeAlgorithm<string> Create(IEdgeSet<string, IEdge<string>> edgeSet, Func<IEdge<string>, double> weights);
  }

  public sealed class CentralityDegreeAlgorithmFactory : ICentralityDegreeAlgorithmFactory
  {
    public ICentralityDegreeAlgorithm<string> Create(IEdgeSet<string, IEdge<string>> edgeSet, Func<IEdge<string>, double> weights)
    {
      return new Degree<string, IEdge<string>>(edgeSet, weights);
    }
  }
}
