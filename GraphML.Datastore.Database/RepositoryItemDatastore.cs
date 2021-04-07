using System;
using System.Collections.Generic;
using Dapper;
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

    public IEnumerable<T> GetParents(Guid itemId, int pageIndex, int pageSize)
    {
      return GetInternal(() =>
      {
        var sql = $"select * from {GetTableName()} where NextId = '{itemId}'";
        return _dbConnection.Query<T>(sql);
      });
    }
  }
}
