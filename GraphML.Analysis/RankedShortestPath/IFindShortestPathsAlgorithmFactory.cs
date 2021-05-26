using QuikGraph;
using System;

namespace GraphML.Analysis.RankedShortestPath
{
  public interface IFindShortestPathsAlgorithmFactory
  {
    IFindShortestPathsAlgorithm<Guid, IdentifiableEdge<Guid>> Create(IBidirectionalGraph<Guid, IdentifiableEdge<Guid>> graph, Func<IdentifiableEdge<Guid>, double> edgeWeights);
  }
}