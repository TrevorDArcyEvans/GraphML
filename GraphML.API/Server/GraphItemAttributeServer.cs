using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class GraphItemAttributeServer : OwnedItemServerBase<GraphItemAttribute>, IGraphItemAttributeServer
  {
    public GraphItemAttributeServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<GraphItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(GraphItemAttribute)}";
  }
}
