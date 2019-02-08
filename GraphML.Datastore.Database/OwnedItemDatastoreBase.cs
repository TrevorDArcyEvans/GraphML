using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GraphML.Datastore.Database
{
  public abstract class OwnedItemDatastoreBase<T> : DatastoreBase<T>, IOwnedDatastore<T> where T : OwnedItem
  {
    public OwnedItemDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<OwnedItemDatastoreBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public virtual IEnumerable<T> ByOwners(IEnumerable<string> ownerIds)
    {
      return GetInternal(() =>
      {
        var sql = $"select * from {GetTableName()} where {nameof(OwnedItem.OwnerId)} in ({GetListIds(ownerIds)})";

        return _dbConnection.Query<T>(sql);
      });
    }
  }
}
