using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

		public IEnumerable<Graph> ByEdgeId(Guid id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Graph> ByNodeId(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
