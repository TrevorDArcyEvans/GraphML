using System;
using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class CentralityDegreeObserver<TVertex>: QuickGraph.Algorithms.Observers.IObserver<ICentralityDegreeAlgorithm<TVertex>>
  {
    private readonly List<DegreeVertexResult<TVertex>> _results = new List<DegreeVertexResult<TVertex>>();

    public IEnumerable<DegreeVertexResult<TVertex>> Results => _results;

    public IDisposable Attach(ICentralityDegreeAlgorithm<TVertex> algorithm)
    {
      algorithm.VertexResult += StoreResult;

      return new DisposableAction(() => algorithm.VertexResult -= StoreResult);
    }

    private void StoreResult(DegreeVertexResult<TVertex> result)
    {
      _results.Add(result);
    }
  }
}
