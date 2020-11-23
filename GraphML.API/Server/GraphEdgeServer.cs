using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
    public sealed class GraphEdgeServer : OwnedItemServerBase<GraphEdge>, IGraphEdgeServer
    {
        public GraphEdgeServer(
            IRestClientFactory clientFactory,
            ILogger<GraphEdgeServer> logger,
            ISyncPolicyFactory policy) :
            base(clientFactory, logger, policy)
        {
        }

        protected override string ResourceBase { get; } = $"/api/{nameof(GraphEdge)}";
    }
}