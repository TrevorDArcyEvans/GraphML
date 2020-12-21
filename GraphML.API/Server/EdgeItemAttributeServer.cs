using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class EdgeItemAttributeServer : OwnedItemServerBase<EdgeItemAttribute>, IEdgeItemAttributeServer
  {
    public EdgeItemAttributeServer(
        IHttpContextAccessor httpContextAccessor,
        IRestClientFactory clientFactory,
        ILogger<EdgeItemAttributeServer> logger,
        ISyncPolicyFactory policy) :
        base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

        protected override string ResourceBase { get; } = $"/api/{nameof(EdgeItemAttribute)}";
  }
}
