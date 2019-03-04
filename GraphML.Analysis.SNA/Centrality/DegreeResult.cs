using GraphML.Interfaces;
using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeResult<TVertex> : IResult
  {
    public string Type => GetType().AssemblyQualifiedName;
    public IEnumerable<DegreeVertexResult<TVertex>> Result { get; }

    public DegreeResult(IEnumerable<DegreeVertexResult<TVertex>> result)
    {
      Result = result;
    }
  }
}
