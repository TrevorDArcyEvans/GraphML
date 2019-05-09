using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class GraphItemAttributeServer : ServerBase<GraphItemAttribute>, IGraphItemAttributeServer
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
