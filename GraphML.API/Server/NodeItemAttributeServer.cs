using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class NodeItemAttributeServer : OwnedItemServerBase<NodeItemAttribute>, INodeItemAttributeServer
  {
    public NodeItemAttributeServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<NodeItemAttributeServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(NodeItemAttribute)}";
  }
}
