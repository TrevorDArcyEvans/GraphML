using System;
using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityClosenessObserver<TVertex> : QuickGraph.Algorithms.Observers.IObserver<ICentralityClosenessAlgorithm<TVertex>>
  {
    private readonly List<ClosenessResult<TVertex>> _results = new List<ClosenessResult<TVertex>>();

    public IEnumerable<ClosenessResult<TVertex>> Results => _results;

    public IDisposable Attach(ICentralityClosenessAlgorithm<TVertex> algorithm)
    {
      algorithm.VertexResult += StoreResult;

      return new DisposableAction(() => algorithm.VertexResult -= StoreResult);
    }

    private void StoreResult(ClosenessResult<TVertex> result)
    {
      _results.Add(result);
    }
  }
}
