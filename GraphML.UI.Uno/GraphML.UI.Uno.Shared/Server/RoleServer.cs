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
			var request = GetPageRequest(Url.Combine(UriResourceBase, "ByContactId", id.ToString()), 1, int.MaxValue);
			var retval = await GetResponse<IEnumerable<Role>>(request);

			return retval;
		}

		public async Task<IEnumerable<Role>> GetAll()
		{
			var request = GetPageRequest(Url.Combine(UriResourceBase, "GetAll"), 1, int.MaxValue);
			var retval = await GetResponse<IEnumerable<Role>>(request);

			return retval;
		}

		public async Task<IEnumerable<Role>> Get()
		{
			var request = GetPageRequest(Url.Combine(UriResourceBase, "Get"), 1, int.MaxValue);
			var retval = await GetResponse<IEnumerable<Role>>(request);

			return retval;
		}
	}
}
