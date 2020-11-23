using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;

namespace GraphML.API.Server
{
	public sealed class GraphServer : OwnedItemServerBase<Graph>, IGraphServer
	{
		public GraphServer(
		  IRestClientFactory clientFactory,
		  ILogger<GraphServer> logger,
		  ISyncPolicyFactory policy) :
		  base(clientFactory, logger, policy)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(Graph)}";

		public async Task<IEnumerable<Graph>> ByEdgeId(Guid id)
		{
			var request = GetAllRequest(Url.Combine(ResourceBase, $"{nameof(GraphController.ByEdgeId)}", id.ToString()));
			var retval = await GetResponse<IEnumerable<Graph>>(request);

			return retval;
		}

		public async Task<IEnumerable<Graph>> ByNodeId(Guid id)
		{
			var request = GetAllRequest(Url.Combine(ResourceBase, $"{nameof(GraphController.ByNodeId)}", id.ToString()));
			var retval = await GetResponse<IEnumerable<Graph>>(request);

			return retval;
		}
	}
}
