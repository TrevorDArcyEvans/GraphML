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
  public sealed class GraphNodeServer : OwnedItemServerBase<GraphNode>, IGraphNodeServer
  {
    public GraphNodeServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<GraphNodeServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(GraphNode)}";
    public async Task<IEnumerable<GraphNode>> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(GraphNodeController.ByRepositoryItems)}", graphId.ToString());
      var request = PostRequest(url, repoItemIds);
      var retval = await RetrieveResponse<IEnumerable<GraphNode>>(request);

      return retval;
    }

    public async Task<IEnumerable<GraphNode>> AddByFilter(Guid graphId, string filter)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(GraphNodeController.AddByFilter)}", graphId.ToString());
      var request = PostRequest(url, filter);
      var retval = await RetrieveResponse<IEnumerable<GraphNode>>(request);

      return retval;
    }
  }
}
