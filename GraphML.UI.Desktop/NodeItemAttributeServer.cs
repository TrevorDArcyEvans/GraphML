using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class NodeItemAttributeServer : ServerBase<NodeItemAttribute>, INodeItemAttributeServer
  {
    public NodeItemAttributeServer(
      IRestClientFactory clientFactory,
      ILogger<NodeItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/NodeItemAttribute";
  }
}
