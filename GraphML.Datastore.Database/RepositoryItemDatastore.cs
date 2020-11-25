using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
	public abstract class RepositoryItemDatastore<T> : OwnedItemDatastoreBase<T>, IRepositoryItemDatastore<T> where T : RepositoryItem
	{
		protected RepositoryItemDatastore(
			IDbConnectionFactory dbConnectionFactory,
			ILogger<OwnedItemDatastoreBase<T>> logger,
			ISyncPolicyFactory policy) :
			base(dbConnectionFactory, logger, policy)
		{
		}

		public IEnumerable<T> GetParents(T entity, int pageIndex, int pageSize)
		{
			return GetInternal(() =>
			{
				return _dbConnection
					.GetAll<T>()
					.Where(x => x.NextId == entity.Id);
			});
		}
	}
}
