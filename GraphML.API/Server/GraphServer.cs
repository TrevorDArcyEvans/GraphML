using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;

namespace GraphML.API.Server
{
  public sealed class GraphServer : OwnedItemServerBase<Graph>, IGraphServer
  {
    public GraphServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<GraphServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(Graph)}";

    public async Task<IEnumerable<Graph>> ByEdgeId(Guid id, int pageIndex, int pageSize)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(GraphController.ByEdgeId)}", id.ToString()), pageIndex, pageSize);
      var retval = await GetResponse<IEnumerable<Graph>>(request);

      return retval;
    }

    public async Task<IEnumerable<Graph>> ByNodeId(Guid id, int pageIndex, int pageSize)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(GraphController.ByNodeId)}", id.ToString()), pageIndex, pageSize);
      var retval = await GetResponse<IEnumerable<Graph>>(request);

      return retval;
    }
  }
}
