namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class ClosenessResult<TVertex>
  {
    public TVertex Vertex { get; }
    public double Closeness { get; }

    public ClosenessResult(TVertex vertex, double closeness)
    {
      Vertex = vertex;
      Closeness = closeness;
    }
  }
}
