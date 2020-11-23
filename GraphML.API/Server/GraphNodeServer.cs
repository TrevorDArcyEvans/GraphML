using GraphML.Interfaces.Server;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
    public sealed class GraphNodeServer : OwnedItemServerBase<GraphNode>, IGraphNodeServer
    {
        public GraphNodeServer(
            IRestClientFactory clientFactory,
            ILogger<GraphNodeServer> logger,
            ISyncPolicyFactory policy) :
            base(clientFactory, logger, policy)
        {
        }

        protected override string ResourceBase { get; } = $"/api/{nameof(GraphNode)}";
    }
}