using Flurl;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;

namespace GraphML.API.Server
{
  public abstract class ItemServerBase<T> : ServerBase, IItemServerBase<T>
  {
    public ItemServerBase(
      IRestClientFactory clientFactory, 
      ILogger<ItemServerBase<T>> logger, 
      ISyncPolicyFactory policy) : 
      base(clientFactory, logger, policy)
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
  }
}
