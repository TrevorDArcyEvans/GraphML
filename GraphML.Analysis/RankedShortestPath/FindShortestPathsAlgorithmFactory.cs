using QuikGraph;
using System;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsAlgorithmFactory : IFindShortestPathsAlgorithmFactory
  {
    public IFindShortestPathsAlgorithm<Guid, IdentifiableEdge<Guid>> Create(IBidirectionalGraph<Guid, IdentifiableEdge<Guid>> graph, Func<IdentifiableEdge<Guid>, double> edgeWeights)
    {
      return new FindShortestPaths<Guid, IdentifiableEdge<Guid>>(graph, edgeWeights);
    }
  }
}