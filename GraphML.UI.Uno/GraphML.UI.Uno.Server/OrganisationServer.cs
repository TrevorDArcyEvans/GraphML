using System.Net.Http;
using GraphML.API.Server;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public sealed class OrganisationServer : ItemServerBase<Organisation>, IOrganisationServer
	{
		public OrganisationServer(
	  IConfiguration config,
			string token,
	  HttpMessageHandler innerHandler,
	  ISyncPolicyFactory policy) :
	  base(config, token, innerHandler, policy)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(Organisation)}";
	}
}
