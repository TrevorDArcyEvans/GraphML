using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
    public sealed class GraphEdgeServer : OwnedItemServerBase<GraphEdge>, IGraphEdgeServer
    {
        public GraphEdgeServer(
            IHttpContextAccessor httpContextAccessor,
            IRestClientFactory clientFactory,
            ILogger<GraphEdgeServer> logger,
            ISyncPolicyFactory policy) :
            base(httpContextAccessor, clientFactory, logger, policy)
        {
        }

        protected override string ResourceBase { get; } = $"/api/{nameof(GraphEdge)}";
    }
}