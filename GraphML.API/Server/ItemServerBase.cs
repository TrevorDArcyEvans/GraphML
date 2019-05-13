using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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

    public IEnumerable<T> ByIds(IEnumerable<string> ids)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, "ByIds"), ids);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Create(IEnumerable<T> entity)
    {
      var request = GetPostRequest(ResourceBase, entity);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Delete(IEnumerable<T> entity)
    {
      var request = GetDeleteRequest(ResourceBase, entity);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Update(IEnumerable<T> entity)
    {
      var request = GetPutRequest(ResourceBase, entity);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }
  }
}
