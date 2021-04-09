using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class RepositoryItemAttributeDefinitionServer : OwnedItemServerBase<RepositoryItemAttributeDefinition>, IRepositoryItemAttributeDefinitionServer
  {
    public RepositoryItemAttributeDefinitionServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<RepositoryItemAttributeDefinitionServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(RepositoryItemAttributeDefinition)}";
  }
}
