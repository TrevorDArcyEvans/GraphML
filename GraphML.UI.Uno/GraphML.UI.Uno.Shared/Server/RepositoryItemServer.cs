using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public abstract class RepositoryItemServer<T> : OwnedItemServerBase<T>, IRepositoryItemServer<T> where T : RepositoryItem
	{
		public RepositoryItemServer(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(config, token, innerHandler)
		{
		}

		public async Task<IEnumerable<T>> GetParents(Guid itemId, int pageIndex, int pageSize)
		{
			var request = GetPageRequest(Url.Combine(UriResourceBase, "GetParents", itemId.ToString()), pageIndex, pageSize);
			var retval = await GetResponse<IEnumerable<T>>(request);

			return retval;
		}
	}
}
