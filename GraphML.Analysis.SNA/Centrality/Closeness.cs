using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.ShortestPath;
using System;
using System.Linq;
using System.Threading;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class Closeness<TVertex, TEdge> : ICentralityClosenessAlgorithm<TVertex> where TEdge : IEdge<TVertex>
  {
    private readonly IVertexListGraph<TVertex, TEdge> _graph;
    private readonly Func<TEdge, double> _weights;

    public event ClosenessResultAction<TVertex> VertexResult;

    private readonly object _syncRoot = new object();

    public Closeness(IVertexListGraph<TVertex, TEdge> graph, Func<TEdge, double> weights)
    {
      _graph = graph;
      _weights = weights;
    }

    public void Compute()
    {
      var toProcess = _graph.Vertices.Count();
      if (toProcess == 0)
      {
        return;
      }

      using (var resetEvent = new ManualResetEvent(false))
      {
        foreach (var node in _graph.Vertices)
        {
          ThreadPool.QueueUserWorkItem(x =>
          {
            var algo = new DijkstraShortestPathAlgorithm<TVertex, TEdge>(_graph, _weights);
            var distObserver = new VertexDistanceRecorderObserver<TVertex, TEdge>(_weights);
            using (var distObserverDisp = distObserver.Attach(algo))
            {
              algo.Compute(node);
            }

            var distances = distObserver.Distances.Sum(dist => dist.Value);
            var closeness = (_graph.Vertices.Count() - 1) / distances;

            OnVertexResult(new ClosenessVertexResult<TVertex>(node, closeness));

            // Safely decrement the counter
            if (Interlocked.Decrement(ref toProcess) == 0)
            {
              resetEvent.Set();
            }
          });
        }
        resetEvent.WaitOne();
      }
    }

    private void OnVertexResult(ClosenessVertexResult<TVertex> result)
    {
      lock (_syncRoot)
      {
        VertexResult?.Invoke(result);
      }
    }
  }
}
