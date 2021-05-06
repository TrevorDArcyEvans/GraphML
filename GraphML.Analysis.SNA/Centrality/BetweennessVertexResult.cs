namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class BetweennessVertexResult<TVertex>
  {
    public TVertex Vertex { get; set; }
    public double Betweenness { get; set; }

    public BetweennessVertexResult(TVertex vertex, double betweenness)
    {
      Vertex = vertex;
      Betweenness = betweenness;
    }
  }
}
