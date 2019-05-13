using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GraphML.API.Server
{
  public sealed class EdgeServer : OwnedItemServerBase<Edge>, IEdgeServer
  {
    public EdgeServer(
      IRestClientFactory clientFactory,
      ILogger<EdgeServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Edge";

    public IEnumerable<Edge> ByNodeIds(IEnumerable<string> ids)
    {
      var request = GetAllRequest(Url.Combine(ResourceBase, "ByNodeIds"));
      var retval = GetResponse<IEnumerable<Edge>>(request);

      return retval;
    }
  }
}
