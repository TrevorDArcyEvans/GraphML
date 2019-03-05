using QuickGraph;
using System;

namespace GraphML.Analysis.RankedShortestPath
{
  public interface IFindShortestPathsAlgorithmFactory
  {
    IFindShortestPathsAlgorithm<string, IEdge<string>> Create(IBidirectionalGraph<string, IEdge<string>> graph, Func<IEdge<string>, double> edgeWeights);
  }
}