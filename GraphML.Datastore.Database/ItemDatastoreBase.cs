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
  public abstract class ItemDatastoreBase<T> : IItemDatastore<T> where T : Item
  {
    protected readonly IDbConnection _dbConnection;
    protected readonly ILogger<IItemDatastore<T>> _logger;
    private readonly ISyncPolicy _policy;

    public ItemDatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ItemDatastoreBase<T>> logger,
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

    protected string AppendForFetch(int pageIndex, int pageSize)
    {
      var dbType = _dbConnection.GetType().ToString();
      switch (dbType)
      {
        case "System.Data.SQLite.SQLiteConnection":
          return $"{Environment.NewLine} limit {pageSize} offset {pageIndex * pageSize}";

        case "MySql.Data.MySqlClient.MySqlConnection":
          return $"{Environment.NewLine} limit {pageIndex * pageSize}, {pageSize}";

        case "Npgsql.NpgsqlConnection":
          return $"{Environment.NewLine} limit {pageSize} offset {pageIndex * pageSize}";

        case "System.Data.SqlClient.SqlConnection":
          return $"{Environment.NewLine} offset {pageIndex * pageSize} rows fetch next {pageSize} rows only";

        default:
          throw new ArgumentOutOfRangeException($"Untested database: {dbType}");
      }
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
