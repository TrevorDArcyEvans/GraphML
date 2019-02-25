using System.Collections.Generic;

namespace GraphML.Analysis.SNA.Centrality
{
  public interface ICentralityBetweennessAlgorithm<TVertex>
  {
    IEnumerable<BetweennessResult<TVertex>> Compute();
  }
}
