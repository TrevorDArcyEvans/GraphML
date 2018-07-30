using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Datastore.Database
{
  public abstract class OwnedItemDatastoreBase<T> : DatastoreBase<T> where T : OwnedItem
  {
    public OwnedItemDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<OwnedItemDatastoreBase<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public override IEnumerable<T> ByIds(IEnumerable<string> ids)
    {
      return GetInternal(() =>
      {
        return _dbConnection.GetAll<T>().Where(x => ids.Contains(x.Id));
      });
    }

    public override IEnumerable<T> ByOwners(IEnumerable<string> ownerIds)
    {
      return GetInternal(() =>
      {
        return _dbConnection.GetAll<T>().Where(x => ownerIds.Contains(x.OwnerId));
      });
    }

    public override IEnumerable<T> Create(IEnumerable<T> entity)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          foreach (var ent in entity)
          {
            ent.Id = ent.Id == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : ent.Id;
          }

          _dbConnection.Insert(entity, trans);
          trans.Commit();

          return entity;
        }
      });
    }

    public override void Delete(IEnumerable<T> entity)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          _dbConnection.Delete(entity, trans);
          trans.Commit();

          return 0;
        }
      });
    }

    public override void Update(IEnumerable<T> entity)
    {
      GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          _dbConnection.Update(entity, trans);
          trans.Commit();

          return 0;
        }
      });
    }
  }
}
