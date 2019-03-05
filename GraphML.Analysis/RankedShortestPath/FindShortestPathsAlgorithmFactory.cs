using QuickGraph;
using System;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsAlgorithmFactory : IFindShortestPathsAlgorithmFactory
  {
    public IFindShortestPathsAlgorithm<string, IEdge<string>> Create(IBidirectionalGraph<string, IEdge<string>> graph, Func<IEdge<string>, double> edgeWeights)
    {
      return new FindShortestPaths<string, IEdge<string>>(graph, edgeWeights);
    }
  }
}