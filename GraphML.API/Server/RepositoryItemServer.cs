using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
    public abstract class RepositoryItemServer<T> : OwnedItemServerBase<T>, IRepositoryItemServer<T> where T : RepositoryItem
    {
        public RepositoryItemServer(
            IHttpContextAccessor httpContextAccessor,
            IRestClientFactory clientFactory,
            ILogger<RepositoryItemServer<T>> logger,
            ISyncPolicyFactory policy) :
            base(httpContextAccessor, clientFactory, logger, policy)
        {
        }

        public async Task<IEnumerable<T>> GetParents(T entity, int pageIndex, int pageSize)
        {
            var request = GetPostRequest(Url.Combine(ResourceBase, nameof(RepositoryItemController<T>.GetParents)), entity); //TODO paging
            var retval = await GetResponse<IEnumerable<T>>(request);

            return retval;
        }
    }
}