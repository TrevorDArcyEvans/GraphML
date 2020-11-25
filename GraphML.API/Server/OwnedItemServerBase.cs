using Flurl;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;

namespace GraphML.API.Server
{
	public abstract class OwnedItemServerBase<T> : ItemServerBase<T>, IOwnedItemServerBase<T> where T : OwnedItem
	{
		public OwnedItemServerBase(
		  IRestClientFactory clientFactory,
		  ILogger<OwnedItemServerBase<T>> logger,
		  ISyncPolicyFactory policy) :
		  base(clientFactory, logger, policy)
		{
		}

		public async Task<IEnumerable<T>> ByOwners(IEnumerable<Guid> ownerIds)
		{
			var request = GetPostRequest(Url.Combine(ResourceBase, nameof(OwnedGraphMLController<T>.ByOwners)), ownerIds);
			var retval = await GetResponse<IEnumerable<T>>(request);

			return retval;
		}
	}
}
