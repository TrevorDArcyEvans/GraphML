using QuickGraph;
using QuickGraph.Algorithms.RankedShortestPath;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPaths<TVertex, TEdge> where TEdge : IEdge<TVertex>
  {
    private readonly IBidirectionalGraph<TVertex, TEdge> _graph;
    private readonly Func<TEdge, double> _edgeWeights;

    public int ShortestPathCount { get; set; } = 10;

    public FindShortestPaths(IBidirectionalGraph<TVertex, TEdge> graph, Func<TEdge, double> edgeWeights)
    {
      _graph = graph;
      _edgeWeights = edgeWeights;
    }

    public IEnumerable<FindShortestPathsResult<TEdge>> GetShortestPaths(TVertex rootVertex, TVertex goalVertex)
    {
      var algo = new HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge>(_graph, _edgeWeights)
      {
        ShortestPathCount = ShortestPathCount
      };
      algo.Compute(rootVertex, goalVertex);

      return algo.ComputedShortestPaths.Select(path => new FindShortestPathsResult<TEdge>(path, path.Sum(e => _edgeWeights(e))));
    }
  }
}
