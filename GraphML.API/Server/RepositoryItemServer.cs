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
  public abstract class RepositoryItemServer<T> : OwnedItemServerBase<T>, IRepositoryItemServer<T> where T : RepositoryItem
  {
    public RepositoryItemServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<RepositoryItemServer<T>> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    public async Task<PagedDataEx<T>> GetParents(Guid itemId, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, nameof(RepositoryItemController<T>.GetParents), itemId.ToString()), pageIndex, pageSize, searchTerm);
      var retval = await RetrieveResponse<PagedDataEx<T>>(request);

      return retval;
    }
  }
}
