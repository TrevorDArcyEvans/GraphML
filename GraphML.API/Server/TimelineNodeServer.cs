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
  public sealed class TimelineNodeServer : OwnedItemServerBase<TimelineNode>, ITimelineNodeServer
  {
    public TimelineNodeServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<TimelineNodeServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(TimelineNode)}";
    
    public async Task<IEnumerable<TimelineNode>> ByGraphItems(Guid timelineId, IEnumerable<Guid> graphItems)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(TimelineNodeController.ByGraphItems)}", timelineId.ToString());
      var request = PostRequest(url, graphItems);
      var retval = await RetrieveResponse<IEnumerable<TimelineNode>>(request);

      return retval;
    }
  }
}
