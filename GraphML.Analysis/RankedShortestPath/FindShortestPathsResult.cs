using System.Collections.Generic;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsResult<TEdge>
  {
    public IEnumerable<TEdge> Path { get; set; }
    public double Cost { get; set; }

    public FindShortestPathsResult(IEnumerable<TEdge> path, double cost)
    {
      Path = path;
      Cost = cost;
    }
  }
}
