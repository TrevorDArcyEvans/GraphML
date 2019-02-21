namespace GraphML.Analysis.SNA.Centrality
{
  public sealed class DegreeResult<TVertex>
  {
    public TVertex Vertex { get; }
    public double In { get; }
    public double Out { get; }

    public DegreeResult(TVertex vertex, double inDegree, double outDegree)
    {
      Vertex = vertex;
      In = inDegree;
      Out = outDegree;
    }
  }
}
