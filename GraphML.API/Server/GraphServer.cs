using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GraphML.API.Server
{
  public sealed class GraphServer : OwnedItemServerBase<Graph>, IGraphServer
  {
    public GraphServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<GraphServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(Graph)}";

    public async Task<PagedDataEx<Graph>> ByEdgeId(Guid id, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(GraphController.ByEdgeId)}", id.ToString()), pageIndex, pageSize, searchTerm);
      var retval = await RetrieveResponse<PagedDataEx<Graph>>(request);

      return retval;
    }

    public async Task<PagedDataEx<Graph>> ByNodeId(Guid id, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(GraphController.ByNodeId)}", id.ToString()), pageIndex, pageSize, searchTerm);
      var retval = await RetrieveResponse<PagedDataEx<Graph>>(request);

      return retval;
    }
  }
}
