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
  public sealed class EdgeServer : RepositoryItemServer<Edge>, IEdgeServer
  {
    public EdgeServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<EdgeServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(Edge)}";

    public async Task<PagedDataEx<Edge>> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(EdgeController.ByNodeIds)}"), pageIndex, pageSize, searchTerm);
      var retval = await GetResponse<PagedDataEx<Edge>>(request);

      return retval;
    }
  }
}
