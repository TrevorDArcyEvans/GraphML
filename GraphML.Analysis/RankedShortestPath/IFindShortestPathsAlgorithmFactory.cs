using QuickGraph;
using System;

namespace GraphML.Analysis.RankedShortestPath
{
  public interface IFindShortestPathsAlgorithmFactory
  {
    IFindShortestPathsAlgorithm<Guid, IEdge<Guid>> Create(IBidirectionalGraph<Guid, IEdge<Guid>> graph, Func<IEdge<Guid>, double> edgeWeights);
  }
}