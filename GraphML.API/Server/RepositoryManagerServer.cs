using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public sealed class RepositoryManagerServer : OwnedItemServerBase<RepositoryManager>, IRepositoryManagerServer
  {
    public RepositoryManagerServer(
      IRestClientFactory clientFactory,
      ILogger<RepositoryManagerServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/RepositoryManager";

    public async Task<IEnumerable<RepositoryManager>> GetAll()
    {
      var request = GetAllRequest(Url.Combine(ResourceBase, "GetAll"));
      var retval = await GetResponse<IEnumerable<RepositoryManager>>(request);

      return retval;
    }
  }
}
