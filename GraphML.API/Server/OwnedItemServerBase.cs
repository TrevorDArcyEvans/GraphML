using Flurl;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;

namespace GraphML.API.Server
{
  public abstract class OwnedItemServerBase<T> : ItemServerBase<T>, IOwnedItemServerBase<T> where T : OwnedItem
  {
    public OwnedItemServerBase(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<OwnedItemServerBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    public async Task<IEnumerable<T>> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex, int pageSize)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, nameof(OwnedGraphMLController<T>.ByOwners)), ownerIds, pageIndex, pageSize);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public async Task<int> Count(Guid ownerId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(OwnedGraphMLController<T>.Count), ownerId.ToString()));
      var retval = await GetResponse<int>(request);

      return retval;
    }
  }
}
