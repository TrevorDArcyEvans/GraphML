using System;
using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityClosenessObserver<TVertex> : QuikGraph.Algorithms.Observers.IObserver<ICentralityClosenessAlgorithm<TVertex>>
  {
    private readonly List<ClosenessVertexResult<TVertex>> _results = new List<ClosenessVertexResult<TVertex>>();

    public IEnumerable<ClosenessVertexResult<TVertex>> Results => _results;

    public IDisposable Attach(ICentralityClosenessAlgorithm<TVertex> algorithm)
    {
      algorithm.VertexResult += StoreResult;

      return new DisposableAction(() => algorithm.VertexResult -= StoreResult);
    }

    private void StoreResult(ClosenessVertexResult<TVertex> result)
    {
      _results.Add(result);
    }
  }
}
