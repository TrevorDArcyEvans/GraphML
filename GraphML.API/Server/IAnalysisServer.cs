using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;

namespace GraphML.UI.Desktop
{
  public interface IAnalysisServer : IServerBase
  {
    string Degree(DegreeRequest req);
    string Closeness(ClosenessRequest req);
    string Betweenness(BetweennessRequest req);
    string FindShortestPaths(FindShortestPathsRequest req);
  }
}
