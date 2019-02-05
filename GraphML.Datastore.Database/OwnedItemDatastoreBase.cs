using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
        return _dbConnection.GetAll<T>().Where(x => ownerIds.Contains(x.OwnerId));
      });
    }
  }
}
