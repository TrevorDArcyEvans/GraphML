﻿using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using System;

namespace GraphML.API.Server
{
  public sealed class EdgeServer : RepositoryItemServer<Edge>, IEdgeServer
  {
    public EdgeServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<EdgeServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(Edge)}";

    public async Task<IEnumerable<Edge>> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(EdgeController.ByNodeIds)}"), pageIndex, pageSize, searchTerm);
      var retval = await GetResponse<IEnumerable<Edge>>(request);

      return retval;
    }
  }
}
