using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public abstract class GraphItemServerBase<T> : OwnedItemServerBase<T>, IGraphItemServer<T> where T : GraphItem
  {
    public GraphItemServerBase(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<GraphItemServerBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }
    
    public async Task<IEnumerable<T>> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(IGraphItemServer<T>.ByRepositoryItems)}", graphId.ToString());
      var request = PostRequest(url, repoItemIds);
      var retval = await RetrieveResponse<IEnumerable<T>>(request);

      return retval;
    }

    public async Task<IEnumerable<T>> AddByFilter(Guid graphId, string filter)
    {
      var url = Url.Combine(ResourceBase, $"{nameof(IGraphItemServer<T>.AddByFilter)}", graphId.ToString());
      var request = PostRequest(url, filter);
      var retval = await RetrieveResponse<IEnumerable<T>>(request);

      return retval;
    }
  }
}
