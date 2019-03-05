using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class BetweennessResult<TVertex> : ResultBase
  {
    public IEnumerable<BetweennessVertexResult<TVertex>> Result { get; }

    public BetweennessResult(IEnumerable<BetweennessVertexResult<TVertex>> result)
    {
      Result = result;
    }
  }
}
