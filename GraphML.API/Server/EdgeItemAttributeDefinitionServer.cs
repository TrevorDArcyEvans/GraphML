using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
	public sealed class EdgeItemAttributeDefinitionServer : OwnedItemServerBase<EdgeItemAttributeDefinition>, IEdgeItemAttributeDefinitionServer
	{
		public EdgeItemAttributeDefinitionServer(
			IHttpContextAccessor httpContextAccessor,
			IRestClientFactory clientFactory,
			ILogger<EdgeItemAttributeDefinitionServer> logger,
			ISyncPolicyFactory policy) :
			base(httpContextAccessor, clientFactory, logger, policy)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(EdgeItemAttributeDefinition)}";
	}
}
