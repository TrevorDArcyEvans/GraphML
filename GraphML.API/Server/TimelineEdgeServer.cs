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
  public sealed class TimelineEdgeServer : OwnedItemServerBase<TimelineEdge>, ITimelineEdgeServer
  {
    public TimelineEdgeServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<TimelineEdgeServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(TimelineEdge)}";
    
    public async Task<IEnumerable<TimelineEdge>> ByGraphItems(Guid timelineId, IEnumerable<Guid> graphItems)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(TimelineEdgeController.ByGraphItems)}", timelineId.ToString());
      var request = PostRequest(url, graphItems);
      var retval = await RetrieveResponse<IEnumerable<TimelineEdge>>(request);

      return retval;
    }
  }
}
