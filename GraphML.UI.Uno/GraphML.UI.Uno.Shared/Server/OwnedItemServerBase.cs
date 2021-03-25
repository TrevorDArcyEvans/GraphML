using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public abstract class OwnedItemServerBase<T> : ItemServerBase<T>, IOwnedItemServerBase<T> where T : OwnedItem
	{
		public OwnedItemServerBase(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(config, token, innerHandler)
		{
		}

		public async Task<IEnumerable<T>> ByOwners(IEnumerable<Guid> ownerIds)
		{
			var request = GetPostRequest(Url.Combine(UriResourceBase, "ByOwners"), ownerIds);
			var retval = await GetResponse<IEnumerable<T>>(request);

			return retval;
		}
	}
}
