using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using System.Threading.Tasks;
using GraphML.Interfaces.Server;

namespace GraphML.API.Server
{
  public interface IAnalysisServer : IServerBase
  {
    Task<string> Degree(DegreeRequest req);
    Task<string> Closeness(ClosenessRequest req);
    Task<string> Betweenness(BetweennessRequest req);
    Task<string> FindShortestPaths(FindShortestPathsRequest req);
  }
}
