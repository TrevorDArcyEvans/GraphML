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

		public async Task<IEnumerable<T>> GetParents(T entity, int pageIndex, int pageSize)
		{
			var request = GetPostRequest(Url.Combine(UriResourceBase, "GetParents"), entity, pageIndex, pageSize);
			var retval = await GetResponse<IEnumerable<T>>(request);

			return retval;
		}
	}
}
