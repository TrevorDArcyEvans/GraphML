using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class NodeItemAttributeDefinitionServer : OwnedItemServerBase<NodeItemAttributeDefinition>, INodeItemAttributeDefinitionServer
  {
    public NodeItemAttributeDefinitionServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<NodeItemAttributeDefinitionServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(NodeItemAttributeDefinition)}";
  }
}
