using Flurl;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GraphML.API.Server
{
  public abstract class OwnedItemServerBase<T> : ItemServerBase<T>, IOwnedItemServerBase<T> where T : OwnedItem
  {
    public OwnedItemServerBase(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<OwnedItemServerBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    public async Task<PagedDataEx<T>> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex, int pageSize, string searchTerm)
    {
      var request = PostPageRequest(Url.Combine(ResourceBase, nameof(OwnedGraphMLController<T>.ByOwners)), ownerIds, pageIndex, pageSize, searchTerm);
      var retval = await RetrieveResponse<PagedDataEx<T>>(request);

      return retval;
    }

    public async Task<PagedDataEx<T>> ByOwner(Guid ownerId, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, nameof(OwnedGraphMLController<T>.ByOwner), ownerId.ToString()), pageIndex, pageSize, searchTerm);
      var retval = await RetrieveResponse<PagedDataEx<T>>(request);

      return retval;
    }

    public async Task<int> Count(Guid ownerId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(OwnedGraphMLController<T>.Count), ownerId.ToString()));
      var retval = await RetrieveResponse<int>(request);

      return retval;
    }
  }
}
