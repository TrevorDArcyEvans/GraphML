using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;

namespace GraphML.API.Server
{
  public sealed class EdgeServer : RepositoryItemServer<Edge>, IEdgeServer
  {
    public EdgeServer(
      IRestClientFactory clientFactory,
      ILogger<EdgeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(Edge)}";

    public async Task<IEnumerable<Edge>> ByNodeIds(IEnumerable<string> ids)
    {
      var request = GetAllRequest(Url.Combine(ResourceBase, $"{nameof(EdgeController.ByNodeIds)}"));
      var retval = await GetResponse<IEnumerable<Edge>>(request);

      return retval;
    }
  }
}
