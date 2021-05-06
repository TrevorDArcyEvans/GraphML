namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class ClosenessVertexResult<TVertex>
  {
    public TVertex Vertex { get; set; }
    public double Closeness { get; set; }

    public ClosenessVertexResult(TVertex vertex, double closeness)
    {
      Vertex = vertex;
      Closeness = closeness;
    }
  }
}
