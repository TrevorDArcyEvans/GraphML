using QuickGraph;
using QuickGraph.Algorithms.RankedShortestPath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GraphML.Analysis.SNA.Centrality
{
  /// <summary>
  /// For an unweighted graph, 'betweenness' is the number of times
  /// a node appears on all shortest paths.
  /// </summary>
  /// <typeparam name="TVertex"></typeparam>
  /// <typeparam name="TEdge"></typeparam>
  public sealed class Betweenness<TVertex, TEdge> : ICentralityBetweennessAlgorithm<TVertex> where TEdge : IEdge<TVertex>
  {
    private object _syncRoot { get; } = new object();
    private readonly IBidirectionalGraph<TVertex, TEdge> _graph;
    Func<TEdge, double> _weights;

    public Betweenness(IBidirectionalGraph<TVertex, TEdge> graph, Func<TEdge, double> weights)
    {
      _graph = graph;
      _weights = weights;
    }

    public IEnumerable<BetweennessResult<TVertex>> Compute()
    {
      var toProcess = _graph.Vertices.Count();
      if (toProcess == 0)
      {
        return Enumerable.Empty<BetweennessResult<TVertex>>();
      }

      var nodeOccurrences = new List<TVertex>();
      using (var resetEvent = new ManualResetEvent(false))
      {
        foreach (var node in _graph.Vertices)
        {
          ThreadPool.QueueUserWorkItem(x =>
          {
            var otherNodes = _graph.Vertices.Except(new[] { node });
            foreach (var otherNode in otherNodes)
            {
              var algo = new HoffmanPavleyRankedShortestPathAlgorithm<TVertex, TEdge>(_graph, _weights) { ShortestPathCount = 10 };
              algo.Compute(node, otherNode);
              if (!algo.ComputedShortestPaths.Any())
              {
                continue;
              }
              var bestPathCost = algo.ComputedShortestPaths.Min(path => path.Sum(edge => _weights(edge)));
              var bestPaths = algo.ComputedShortestPaths.Where(path => path.Sum(edge => _weights(edge)) <= bestPathCost);
              var betweenNodes = bestPaths
                .SelectMany(path => path.SelectMany(edge => new[] { edge.Source, edge.Target }))
                .Except(new[] { node, otherNode });
              lock (_syncRoot)
              {
                nodeOccurrences.AddRange(betweenNodes);
              }
            }

            // Safely decrement the counter
            if (Interlocked.Decrement(ref toProcess) == 0)
            {
              resetEvent.Set();
            }
          });
        }
        resetEvent.WaitOne();
      }

      var retval = from node in nodeOccurrences
                   group node by node into grp
                   select new BetweennessResult<TVertex>(grp.Key, grp.Count());

      return retval.OrderByDescending(x => x.Betweenness);
    }
  }
}
