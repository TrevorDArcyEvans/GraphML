using Flurl;
using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GraphML.API.Controllers;
using System;
using Microsoft.AspNetCore.Http;

namespace GraphML.API.Server
{
  public sealed class AnalysisServer : ServerBase, IAnalysisServer
  {
    public AnalysisServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<AnalysisServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Analysis";

    public async Task<Guid> Betweenness(BetweennessRequest req)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, $"{nameof(AnalysisController.Betweenness)}"), req);
      var retval = await GetResponse<Guid>(request);

      return retval;
    }

    public async Task<Guid> Closeness(ClosenessRequest req)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, $"{nameof(AnalysisController.Closeness)}"), req);
      var retval = await GetResponse<Guid>(request);

      return retval;
    }

    public async Task<Guid> Degree(DegreeRequest req)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, $"{nameof(AnalysisController.Degree)}"), req);
      var retval = await GetResponse<Guid>(request);

      return retval;
    }

    public async Task<Guid> FindShortestPaths(FindShortestPathsRequest req)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, $"{nameof(AnalysisController.FindShortestPaths)}"), req);
      var retval = await GetResponse<Guid>(request);

      return retval;
    }
  }
}
