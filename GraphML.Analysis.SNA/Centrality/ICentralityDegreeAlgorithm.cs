namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityDegreeAlgorithm<TVertex>
  {
    event DegreeResultAction<TVertex> VertexResult;
    void Compute();
  }
}
