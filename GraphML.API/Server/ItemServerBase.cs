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
  public abstract class ItemServerBase<T> : ServerBase, IItemServerBase<T> where T : Item
  {
    public ItemServerBase(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<ItemServerBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    public async Task<IEnumerable<T>> ByIds(IEnumerable<Guid> ids)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, nameof(GraphMLController<T>.ByIds)), ids);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public async Task<IEnumerable<T>> Create(IEnumerable<T> entity)
    {
      var request = GetPostRequest(ResourceBase, entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public async Task<IEnumerable<T>> Delete(IEnumerable<T> entity)
    {
      var request = GetDeleteRequest(ResourceBase, entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public async Task<IEnumerable<T>> Update(IEnumerable<T> entity)
    {
      var request = GetPutRequest(ResourceBase, entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public async Task<int> Count()
    {
      var path = Url.Combine(ResourceBase, nameof(Count));
      var request = GetRequest(path);
      var retval = await GetResponse<int>(request);

      return retval;
    }
  }
}
