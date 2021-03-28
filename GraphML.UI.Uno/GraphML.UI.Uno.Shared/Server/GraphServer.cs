using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public sealed class GraphServer : OwnedItemServerBase<Graph>, IGraphServer
	{
		public GraphServer(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(config, token, innerHandler)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(Graph)}";

		public async Task<IEnumerable<Graph>> ByEdgeId(Guid id, int pageIndex,	int pageSize)
		{
			var request = GetPageRequest(Url.Combine(UriResourceBase, "ByEdgeId", id.ToString()), pageIndex, pageSize);
			var retval = await GetResponse<IEnumerable<Graph>>(request);

			return retval;
		}

		public async Task<IEnumerable<Graph>> ByNodeId(Guid id, int pageIndex,	int pageSize)
		{
			var request = GetPageRequest(Url.Combine(UriResourceBase, "ByNodeId", id.ToString()), pageIndex, pageSize);
			var retval = await GetResponse<IEnumerable<Graph>>(request);

			return retval;
		}
	}
}
