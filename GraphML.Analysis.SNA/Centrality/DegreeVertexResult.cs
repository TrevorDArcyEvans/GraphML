namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeVertexResult<TVertex>
  {
    public TVertex Vertex { get; set; }
    public double In { get; set; }
    public double Out { get; set; }

    public DegreeVertexResult(TVertex vertex, double inDegree, double outDegree)
    {
      Vertex = vertex;
      In = inDegree;
      Out = outDegree;
    }
  }
}
