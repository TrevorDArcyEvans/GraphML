using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public sealed class EdgeServer : RepositoryItemServer<Edge>, IEdgeServer
	{
		public EdgeServer(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(config, token, innerHandler)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(Edge)}";

		public async Task<IEnumerable<Edge>> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize)
		{
			var request = GetAllRequest(Url.Combine(UriResourceBase, "ByNodeIds")); //TODO paging
			var retval = await GetResponse<IEnumerable<Edge>>(request);

			return retval;
		}
	}
}
