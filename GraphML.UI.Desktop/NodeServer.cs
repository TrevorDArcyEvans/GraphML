using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class NodeServer : ServerBase<Node>, INodeServer
  {
    public NodeServer(
      IRestClientFactory clientFactory,
      ILogger<NodeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Node";
  }
}
