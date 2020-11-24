using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using System.Threading.Tasks;
using GraphML.Interfaces.Server;
using System;

namespace GraphML.API.Server
{
  public interface IAnalysisServer : IServerBase
  {
    Task<Guid> Degree(DegreeRequest req);
    Task<Guid> Closeness(ClosenessRequest req);
    Task<Guid> Betweenness(BetweennessRequest req);
    Task<Guid> FindShortestPaths(FindShortestPathsRequest req);
  }
}
