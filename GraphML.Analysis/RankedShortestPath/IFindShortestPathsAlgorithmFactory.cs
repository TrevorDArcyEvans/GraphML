using QuikGraph;
using System;

namespace GraphML.Analysis.RankedShortestPath
{
  public interface IFindShortestPathsAlgorithmFactory
  {
    IFindShortestPathsAlgorithm<Guid, Edge<Guid>> Create(IBidirectionalGraph<Guid, Edge<Guid>> graph, Func<Edge<Guid>, double> edgeWeights);
  }
}