using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class NodeServer : OwnedItemServerBase<Node>, INodeServer
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
