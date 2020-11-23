using GraphML.Interfaces.Server;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class NodeItemAttributeServer : OwnedItemServerBase<NodeItemAttribute>, INodeItemAttributeServer
  {
    public NodeItemAttributeServer(
      IRestClientFactory clientFactory,
      ILogger<NodeItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(NodeItemAttribute)}";
  }
}
