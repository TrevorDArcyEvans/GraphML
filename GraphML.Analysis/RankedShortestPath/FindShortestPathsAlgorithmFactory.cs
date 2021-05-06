using QuikGraph;
using System;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsAlgorithmFactory : IFindShortestPathsAlgorithmFactory
  {
    public IFindShortestPathsAlgorithm<Guid, Edge<Guid>> Create(IBidirectionalGraph<Guid, Edge<Guid>> graph, Func<Edge<Guid>, double> edgeWeights)
    {
      return new FindShortestPaths<Guid, Edge<Guid>>(graph, edgeWeights);
    }
  }
}