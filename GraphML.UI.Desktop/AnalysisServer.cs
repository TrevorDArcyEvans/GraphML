using Flurl;
using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class AnalysisServer : ServerBase, IAnalysisServer
  {
    public AnalysisServer(
      IRestClientFactory clientFactory, 
      ILogger<ServerBase> logger, 
      ISyncPolicyFactory policy) : 
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Analysis";

    public string Betweenness(BetweennessRequest req)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "Betweenness"), req);
      var retval = GetResponse<string>(request);

      return retval;
    }

    public string Closeness(ClosenessRequest req)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "Closeness"), req);
      var retval = GetResponse<string>(request);

      return retval;
    }

    public string Degree(DegreeRequest req)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "Degree"), req);
      var retval = GetResponse<string>(request);

      return retval;
    }

    public string FindShortestPaths(FindShortestPathsRequest req)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "FindShortestPaths"), req);
      var retval = GetResponse<string>(request);

      return retval;
    }
  }
}
