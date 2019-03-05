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
}
