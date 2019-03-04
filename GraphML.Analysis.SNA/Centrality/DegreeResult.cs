namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeVertexResult<TVertex>
  {
    public TVertex Vertex { get; }
    public double In { get; }
    public double Out { get; }

    public DegreeVertexResult(TVertex vertex, double inDegree, double outDegree)
    {
      Vertex = vertex;
      In = inDegree;
      Out = outDegree;
    }
  }
}
