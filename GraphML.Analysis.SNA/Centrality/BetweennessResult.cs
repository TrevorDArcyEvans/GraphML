namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class BetweennessResult<TVertex>
  {
    public TVertex Vertex { get; }
    public double Betweenness { get; }

    public BetweennessResult(TVertex vertex, double betweenness)
    {
      Vertex = vertex;
      Betweenness = betweenness;
    }
  }
}
