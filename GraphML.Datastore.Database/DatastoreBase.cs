using Dapper;
using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using System;
using System.Collections.Generic;
using Schema = System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GraphML.Datastore.Database
{
  public abstract class DatastoreBase<T> : IDatastore<T> where T : Item
  {
    protected readonly IDbConnection _dbConnection;
    protected readonly ILogger<IDatastore<T>> _logger;
    private readonly ISyncPolicy _policy;

    public DatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<DatastoreBase<T>> logger,
      ISyncPolicyFactory policy)
    {
      _dbConnection = dbConnectionFactory.Get();
      _logger = logger;
      _policy = policy.Build(_logger);
    }

    public IEnumerable<T> ByIds(IEnumerable<Guid> ids)
    {
      return GetInternal(() =>
      {
        var sql = $"select * from {GetTableName()} where {nameof(Item.Id)} in ({GetListIds(ids)})";

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

    protected TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
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
