using System.Collections.Generic;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsResult<TEdge>
  {
    public IEnumerable<TEdge> Path { get; }
    public double Cost { get; }

    public FindShortestPathsResult(IEnumerable<TEdge> path, double cost)
    {
      Path = path;
      Cost = cost;
    }
  }
}
