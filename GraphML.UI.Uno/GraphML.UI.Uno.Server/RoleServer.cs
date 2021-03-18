using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public sealed class RoleServer : ItemServerBase<Role>, IRoleServer
	{
		public RoleServer(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(config, token, innerHandler)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(Role)}";

		public async Task<IEnumerable<Role>> ByContactId(Guid id)
		{
			var request = GetAllRequest(Url.Combine(UriResourceBase, "ByContactId", id.ToString()));
			var retval = await GetResponse<IEnumerable<Role>>(request);

			return retval;
		}

		public async Task<IEnumerable<Role>> GetAll()
		{
			var request = GetAllRequest(Url.Combine(UriResourceBase, "GetAll"));
			var retval = await GetResponse<IEnumerable<Role>>(request);

			return retval;
		}

		public async Task<IEnumerable<Role>> Get()
		{
			var request = GetAllRequest(Url.Combine(UriResourceBase, "Get"));
			var retval = await GetResponse<IEnumerable<Role>>(request);

			return retval;
		}
	}
}
