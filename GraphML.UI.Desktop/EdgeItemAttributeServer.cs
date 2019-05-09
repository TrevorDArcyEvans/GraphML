using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class EdgeItemAttributeServer : ServerBase<EdgeItemAttribute>, IEdgeItemAttributeServer
  {
    public EdgeItemAttributeServer(
      IRestClientFactory clientFactory,
      ILogger<EdgeItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/EdgeItemAttribute";
  }
}
