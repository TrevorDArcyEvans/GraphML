using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{

  public sealed class ClosenessResult<TVertex> : ResultBase
  {
    public IEnumerable<ClosenessVertexResult<TVertex>> Result { get; }

    public ClosenessResult(IEnumerable<ClosenessVertexResult<TVertex>> result)
    {
      Result = result;
    }
  }
}
