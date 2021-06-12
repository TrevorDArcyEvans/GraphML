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
  public sealed class GraphEdgeServer : OwnedItemServerBase<GraphEdge>, IGraphEdgeServer
  {
    public GraphEdgeServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<GraphEdgeServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(GraphEdge)}";
    public async Task<IEnumerable<GraphEdge>> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(GraphEdgeController.ByRepositoryItems)}", graphId.ToString());
      var request = PostRequest(url, repoItemIds);
      var retval = await RetrieveResponse<IEnumerable<GraphEdge>>(request);

      return retval;
    }

    public async Task<PagedDataEx<GraphEdge>> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm)
    {
      var request = PostPageRequest(Url.Combine(ResourceBase, $"{nameof(GraphEdgeController.ByNodeIds)}"), ids, pageIndex, pageSize, searchTerm);
      var retval = await RetrieveResponse<PagedDataEx<GraphEdge>>(request);

      return retval;
    }

    public async Task<IEnumerable<GraphEdge>> AddByFilter(Guid graphId, string filter)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(GraphEdgeController.AddByFilter)}", graphId.ToString());
      var request = PostRequest(url, filter);
      var retval = await RetrieveResponse<IEnumerable<GraphEdge>>(request);

      return retval;
    }
  }
}
