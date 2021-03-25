using System.Net.Http;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public sealed class GraphEdgeServer : OwnedItemServerBase<GraphEdge>, IGraphEdgeServer
	{
		public GraphEdgeServer(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(config, token, innerHandler)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(GraphEdge)}";
	}
}
