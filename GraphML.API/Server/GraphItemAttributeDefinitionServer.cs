using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
	public sealed class GraphItemAttributeDefinitionServer : OwnedItemServerBase<GraphItemAttributeDefinition>, IGraphItemAttributeDefinitionServer
	{
		public GraphItemAttributeDefinitionServer(
			IHttpContextAccessor httpContextAccessor,
			IRestClientFactory clientFactory,
			ILogger<GraphItemAttributeDefinitionServer> logger,
			ISyncPolicyFactory policy) :
			base(httpContextAccessor, clientFactory, logger, policy)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(GraphItemAttributeDefinition)}";
	}
}
