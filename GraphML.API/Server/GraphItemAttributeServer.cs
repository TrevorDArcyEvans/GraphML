using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class GraphItemAttributeServer : OwnedItemServerBase<GraphItemAttribute>, IGraphItemAttributeServer
  {
    public GraphItemAttributeServer(
      IRestClientFactory clientFactory,
      ILogger<GraphItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/GraphItemAttribute";
  }
}
