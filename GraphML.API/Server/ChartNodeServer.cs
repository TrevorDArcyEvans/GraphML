using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class ChartNodeServer : OwnedItemServerBase<ChartNode>, IChartNodeServer
  {
    public ChartNodeServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<ChartNodeServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(ChartNode)}";
    public async Task<ChartNode> ByGraphItem(Guid chartId, Guid graphItem)
    {
      var url = Url.Combine(
        ResourceBase, 
        $"{nameof(ChartNodeController.ByGraphItem)}",
        chartId.ToString(),
        graphItem.ToString());
      var request = GetRequest(url);
      var retval = await RetrieveResponse<ChartNode>(request);

      return retval;
    }
  }
}
