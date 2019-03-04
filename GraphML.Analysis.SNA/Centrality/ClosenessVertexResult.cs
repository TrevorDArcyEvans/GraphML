namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class ClosenessVertexResult<TVertex>
  {
    public TVertex Vertex { get; }
    public double Closeness { get; }

    public ClosenessVertexResult(TVertex vertex, double closeness)
    {
      Vertex = vertex;
      Closeness = closeness;
    }
  }
}
