using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class RepositoryItemAttributeServer : OwnedItemServerBase<RepositoryItemAttribute>, IRepositoryItemAttributeServer
  {
    public RepositoryItemAttributeServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<RepositoryItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(RepositoryItemAttribute)}";
  }
}
