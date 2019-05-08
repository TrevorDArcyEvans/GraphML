using System.Collections.Generic;
using Flurl;
using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class EdgeServer : ServerBase<Edge>, IEdgeServer
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
