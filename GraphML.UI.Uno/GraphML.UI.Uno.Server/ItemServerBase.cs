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
		private string UriBase { get; }
		protected abstract string ResourceBase { get; }

		protected ItemServerBase(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(token, innerHandler)
		{
			UriBase = config.API_URI();
    }

		public async Task<IEnumerable<T>> ByIds(IEnumerable<Guid> ids)
		{
      var request = GetPostRequest(Url.Combine(UriBase, ResourceBase, "ByIds"), ids);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
		}

		public async Task<IEnumerable<T>> Create(IEnumerable<T> entity)
		{
      var request = GetPostRequest(Url.Combine(UriBase, ResourceBase), entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
		}

		public async Task<IEnumerable<T>> Delete(IEnumerable<T> entity)
		{
      var request = GetDeleteRequest(Url.Combine(UriBase, ResourceBase), entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
		}

		public async Task<IEnumerable<T>> Update(IEnumerable<T> entity)
		{
      var request = GetPutRequest(Url.Combine(UriBase, ResourceBase), entity);
      var retval = await GetResponse<IEnumerable<T>>(request);

      return retval;
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			var request = GetAllRequest(Url.Combine(UriBase, ResourceBase, "GetAll"));
			var retval = await GetResponse<IEnumerable<T>>(request);

			return retval;
		}
	}
}
