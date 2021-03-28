using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.Common;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public abstract class ItemServerBase<T> : ServerBase, IItemServerBase<T> where T : Item
	{
		protected string UriResourceBase { get; }
		protected abstract string ResourceBase { get; }

		protected ItemServerBase(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(token, innerHandler)
		{
			UriResourceBase = Url.Combine(config.API_URI(), ResourceBase);
    }

		public async Task<IEnumerable<T>> ByIds(IEnumerable<Guid> ids)
		{
      var request = GetPostRequest(Url.Combine(UriResourceBase, "ByIds"), ids);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
		}

		public async Task<IEnumerable<T>> Create(IEnumerable<T> entity)
		{
      var request = GetPostRequest(UriResourceBase, entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
		}

		public async Task<IEnumerable<T>> Delete(IEnumerable<T> entity)
		{
      var request = GetDeleteRequest(UriResourceBase, entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
		}

		public async Task<IEnumerable<T>> Update(IEnumerable<T> entity)
		{
      var request = GetPutRequest(UriResourceBase, entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			var request = GetPageRequest(Url.Combine(UriResourceBase, "GetAll"));
			var retval = await GetResponse<IEnumerable<T>>(request);

			return retval;
		}
	}
}
