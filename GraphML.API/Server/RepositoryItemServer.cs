using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
    public abstract class RepositoryItemServer<T> : OwnedItemServerBase<T>, IRepositoryItemServer<T> where T : RepositoryItem
    {
        public RepositoryItemServer(
            IRestClientFactory clientFactory,
            ILogger<RepositoryItemServer<T>> logger,
            ISyncPolicyFactory policy) :
            base(clientFactory, logger, policy)
        {
        }

        public async Task<IEnumerable<T>> GetParents(T entity)
        {
            var request = GetPostRequest(Url.Combine(ResourceBase, nameof(RepositoryItemController<T>.GetParents)), entity);
            var retval = await GetResponse<IEnumerable<T>>(request);

            return retval;
        }
    }
}