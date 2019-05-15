using Flurl;
using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public sealed class AnalysisServer : ServerBase, IAnalysisServer
  {
    public AnalysisServer(
      IRestClientFactory clientFactory, 
      ILogger<AnalysisServer> logger, 
      ISyncPolicyFactory policy) : 
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Analysis";

    public async Task<string> Betweenness(BetweennessRequest req)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "Betweenness"), req);
      var retval = await GetResponse<string>(request);

      return retval;
    }

    public async Task<string> Closeness(ClosenessRequest req)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "Closeness"), req);
      var retval = await GetResponse<string>(request);

      return retval;
    }

    public async Task<string> Degree(DegreeRequest req)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "Degree"), req);
      var retval = await GetResponse<string>(request);

      return retval;
    }

    public async Task<string> FindShortestPaths(FindShortestPathsRequest req)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "FindShortestPaths"), req);
      var retval = await GetResponse<string>(request);

      return retval;
    }
  }
  }
