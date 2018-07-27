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

    public virtual IEnumerable<T> ByIds(IEnumerable<string> ids)
    {
      return GetInternal(() =>
      {
        return _dbConnection.GetAll<T>().Where(x => ids.Contains(x.Id));
      });
    }

    public virtual IEnumerable<T> ByOwners(IEnumerable<string> ownerIds)
    {
      return GetInternal(() =>
      {
        return _dbConnection.GetAll<T>().Where(x => ownerIds.Contains(x.OwnerId));
      });
    }

    public virtual IEnumerable<T> Create(IEnumerable<T> entity)
    {
      return GetInternal(() =>
      {
        using (var trans = _dbConnection.BeginTransaction())
        {
          foreach (var ent in entity)
          {
            ent.Id = Guid.NewGuid().ToString();
          }

          _dbConnection.Insert(entity, trans);
          trans.Commit();

          return entity;
        }
      });
    }

    public virtual void Delete(IEnumerable<T> entity)
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

    public virtual void Update(IEnumerable<T> entity)
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
