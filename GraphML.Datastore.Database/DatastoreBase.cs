using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GraphML.Datastore.Database
{
  public abstract class DatastoreBase<T> : IDatastore<T> where T : OwnedItem
  {
    protected readonly IDbConnection _dbConnection;
    protected readonly ILogger<DatastoreBase<T>> _logger;
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

    public IQueryable<T> ByOwner(string ownerId)
    {
      return GetInternal(() =>
      {
        return _dbConnection.GetAll<T>().Where(x => x.OwnerId == ownerId).AsQueryable();
      });
    }

    public T Create(T entity)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          entity.Id = Guid.NewGuid().ToString();
          _dbConnection.Insert(entity, trans);
          trans.Commit();

          return entity;
        }
      });
    }

    public virtual void Delete(T entity)
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

    public void Update(T entity)
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
  }
}
