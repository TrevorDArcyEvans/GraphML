using System.Collections.Generic;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsResults<TEdge> : ResultBase
  {
    public IEnumerable<FindShortestPathsResult<TEdge>> Result { get; }

    public FindShortestPathsResults(IEnumerable<FindShortestPathsResult<TEdge>> result)
    {
      Result = result;
    }
  }
}
