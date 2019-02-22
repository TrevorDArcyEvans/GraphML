namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityClosenessAlgorithm<TVertex>
  {
    event ClosenessResultAction<TVertex> VertexResult;
  }
}
