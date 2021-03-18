using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Server;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public abstract class ItemServerBase<T> : ServerBase, IItemServerBase<T> where T : Item
	{
		protected ItemServerBase(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler,
			ISyncPolicyFactory policy) :
			base(config, token, innerHandler, policy)
		{
		}

		public Task<IEnumerable<T>> ByIds(IEnumerable<Guid> ids)
		{
		// TODO		ByIds
			throw new NotImplementedException();
		}

		public Task<IEnumerable<T>> Create(IEnumerable<T> entity)
		{
		// TODO		Create
			throw new NotImplementedException();
		}

		public Task<IEnumerable<T>> Delete(IEnumerable<T> entity)
		{
		// TODO		Delete
			throw new NotImplementedException();
		}

		public Task<IEnumerable<T>> Update(IEnumerable<T> entity)
		{
		// TODO		Update
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			var request = GetAllRequest(Url.Combine(UriBase, ResourceBase, "GetAll"));
			var retval = await GetResponse<IEnumerable<T>>(request);

			return retval;
		}
	}
}
