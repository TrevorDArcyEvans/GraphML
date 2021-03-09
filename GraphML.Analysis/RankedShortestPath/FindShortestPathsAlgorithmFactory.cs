using QuikGraph;
using System;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsAlgorithmFactory : IFindShortestPathsAlgorithmFactory
  {
    public IFindShortestPathsAlgorithm<Guid, IEdge<Guid>> Create(IBidirectionalGraph<Guid, IEdge<Guid>> graph, Func<IEdge<Guid>, double> edgeWeights)
    {
      return new FindShortestPaths<Guid, IEdge<Guid>>(graph, edgeWeights);
    }
  }
}