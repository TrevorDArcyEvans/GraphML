using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class BetweennessVertexResult<TVertex>
  {
    public TVertex Vertex { get; }
    public double Betweenness { get; }

    public BetweennessVertexResult(TVertex vertex, double betweenness)
    {
      Vertex = vertex;
      Betweenness = betweenness;
    }
  }
  public sealed class BetweennessResult<TVertex> : ResultBase
  {
    public IEnumerable<BetweennessVertexResult<TVertex>> Result { get; }

    public BetweennessResult(IEnumerable<BetweennessVertexResult<TVertex>> result)
    {
      Result = result;
    }
  }
}
