using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class RepositoryManagerServer : OwnedItemServerBase<RepositoryManager>, IRepositoryManagerServer
  {
    public RepositoryManagerServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<RepositoryManagerServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(RepositoryManager)}";
  }
}
