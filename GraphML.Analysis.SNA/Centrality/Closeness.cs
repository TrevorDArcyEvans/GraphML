using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.ShortestPath;
using System;
using System.Linq;
using System.Threading;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class Closeness<TVertex, TEdge> : IComputation, ICentralityClosenessAlgorithm<TVertex> where TEdge : IEdge<TVertex>
  {
    private readonly IVertexListGraph<TVertex, TEdge> _graph;
    Func<TEdge, double> _weights;

    public event EventHandler StateChanged;
    public event EventHandler Started;
    public event EventHandler Finished;
    public event EventHandler Aborted;

    public event ClosenessResultAction<TVertex> VertexResult;

    public ComputationState State { get; private set; } = ComputationState.NotRunning;

    public object SyncRoot { get; } = new object();

    public Closeness(IVertexListGraph<TVertex, TEdge> graph, Func<TEdge, double> weights)
    {
      _graph = graph;
      _weights = weights;
    }

    public void Abort()
    {
      State = ComputationState.PendingAbortion;
      StateChanged?.Invoke(this, new EventArgs());
    }

    public void Compute()
    {
      try
      {
        State = ComputationState.Running;
        Started?.Invoke(this, new EventArgs());
        StateChanged?.Invoke(this, new EventArgs());

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
                if (State == ComputationState.PendingAbortion)
                {
                  return;
                }
                algo.Compute(node);
              }

              var distances = distObserver.Distances.Sum(dist => dist.Value);
              var closeness = (_graph.Vertices.Count() - 1) / distances;

              lock (SyncRoot)
              {
                OnVertexResult(new ClosenessResult<TVertex>(node, closeness));
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
      }
      finally
      {
        if (State == ComputationState.PendingAbortion)
        {
          State = ComputationState.Aborted;
          Aborted?.Invoke(this, new EventArgs());
          StateChanged?.Invoke(this, new EventArgs());
        }
        else
        {
          State = ComputationState.Finished;
          Finished?.Invoke(this, new EventArgs());
          StateChanged?.Invoke(this, new EventArgs());
        }
      }
    }

    private void OnVertexResult(ClosenessResult<TVertex> result)
    {
      VertexResult?.Invoke(result);
    }
  }
}
