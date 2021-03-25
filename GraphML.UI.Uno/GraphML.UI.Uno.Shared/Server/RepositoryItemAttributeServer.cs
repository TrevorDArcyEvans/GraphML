using System.Net.Http;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public sealed class RepositoryItemAttributeServer : OwnedItemServerBase<RepositoryItemAttribute>, IRepositoryItemAttributeServer
	{
		public RepositoryItemAttributeServer(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(config, token, innerHandler)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(RepositoryItemAttribute)}";
	}
}
