namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityBetweennessAlgorithm<TVertex>
  {
    BetweennessResult<TVertex> Compute();
  }
}
