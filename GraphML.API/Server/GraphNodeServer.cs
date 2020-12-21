using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
    public sealed class GraphNodeServer : OwnedItemServerBase<GraphNode>, IGraphNodeServer
    {
        public GraphNodeServer(
            IHttpContextAccessor httpContextAccessor,
            IRestClientFactory clientFactory,
            ILogger<GraphNodeServer> logger,
            ISyncPolicyFactory policy) :
            base(httpContextAccessor, clientFactory, logger, policy)
        {
        }

        protected override string ResourceBase { get; } = $"/api/{nameof(GraphNode)}";
    }
}