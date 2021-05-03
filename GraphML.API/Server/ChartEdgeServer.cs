using System;
using System.Collections.Generic;
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
  public sealed class ChartEdgeServer : OwnedItemServerBase<ChartEdge>, IChartEdgeServer
  {
    public ChartEdgeServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<ChartEdgeServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(ChartEdge)}";
    
    public async Task<IEnumerable<ChartEdge>> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItems)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(ChartEdgeController.ByGraphItems)}", chartId.ToString());
      var request = PostRequest(url, graphItems);
      var retval = await RetrieveResponse<IEnumerable<ChartEdge>>(request);

      return retval;
    }
  }
}
