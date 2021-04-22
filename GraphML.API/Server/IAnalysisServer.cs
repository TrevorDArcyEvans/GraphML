using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using System.Threading.Tasks;
using System;

namespace GraphML.API.Server
{
  public interface IAnalysisServer
  {
    Task<Guid> Degree(DegreeRequest req);
    Task<Guid> Closeness(ClosenessRequest req);
    Task<Guid> Betweenness(BetweennessRequest req);
    Task<Guid> FindShortestPaths(FindShortestPathsRequest req);
  }
}
