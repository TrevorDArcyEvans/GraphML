using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeResult<TVertex> : ResultBase
  {
    public IEnumerable<DegreeVertexResult<TVertex>> Result { get; }

    public DegreeResult(IEnumerable<DegreeVertexResult<TVertex>> result)
    {
      Result = result;
    }
  }
}
