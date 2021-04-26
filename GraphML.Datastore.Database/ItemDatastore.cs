using Dapper;
using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Schema = System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GraphML.Datastore.Database
{
  public abstract class ItemDatastore<T> : DatastoreBase, IItemDatastore<T> where T : Item
  {
    public ItemDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ItemDatastore<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<T> ByIds(IEnumerable<Guid> ids)
    {
      return GetInternal(() =>
      {
        var sql = $"select * from {GetTableName()} where {nameof(Item.Id)} in ({GetListIds(ids)}) order by {nameof(Item.Name)}";

        return _dbConnection.Query<T>(sql);
      });
    }

    public IEnumerable<T> Create(IEnumerable<T> entity)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          foreach (var ent in entity)
          {
            ent.Id = ent.Id == Guid.Empty ? Guid.NewGuid() : ent.Id;
            _dbConnection.Insert(ent, trans);
          }

          trans.Commit();

          return entity;
        }
      });
    }

    public void Delete(IEnumerable<T> entity)
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

    public void Update(IEnumerable<T> entity)
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

    public int Count()
    {
      return GetInternal(() =>
      {
        var sql = $"select count(*) from {GetTableName()}";

        return _dbConnection.QueryFirst<int>(sql);
      });
    }

    protected string GetLogMessage(IEnumerable<T> infos, [CallerMemberName] string caller = "")
    {
      return caller + " --> " + JArray.FromObject(infos).ToString(Formatting.None);
    }

    protected string GetLogMessage(object info, [CallerMemberName] string caller = "")
    {
      return caller + " --> " + JObject.FromObject(info).ToString(Formatting.None);
    }

    protected static string GetListIds(IEnumerable<Guid> ids)
    {
      var quotedIds = ids.Select(id => $"'{id}'");
      var listIds = string.Join(',', quotedIds.ToArray());

      return listIds;
    }

    protected string GetTableName()
    {
      var tableName = typeof(T).GetCustomAttribute<Schema.TableAttribute>().Name;

      return tableName;
    }
  }
}
