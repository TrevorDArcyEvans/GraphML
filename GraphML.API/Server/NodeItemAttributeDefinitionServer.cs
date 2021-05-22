using System.Net.Http;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class NodeItemAttributeDefinitionServer : OwnedItemServerBase<NodeItemAttributeDefinition>, INodeItemAttributeDefinitionServer
  {
    public NodeItemAttributeDefinitionServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<NodeItemAttributeDefinitionServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(NodeItemAttributeDefinition)}";
  }
}
