using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

namespace GraphML.Datastore.Database
{
  public abstract class DatastoreBase<T> : IDatastore<T>
  {
    protected readonly IDbConnection _dbConnection;
    protected readonly ILogger<IDatastore<T>> _logger;
    private readonly ISyncPolicy _policy;

    public DatastoreBase(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<IDatastore<T>> logger,
      ISyncPolicyFactory policy)
    {
      _dbConnection = dbConnectionFactory.Get();
      _logger = logger;
      _policy = policy.Build(_logger);
    }

    public abstract IEnumerable<T> ByIds(IEnumerable<string> id);
    public abstract IEnumerable<T> ByOwners(IEnumerable<string> ownerIds);
    public abstract IEnumerable<T> Create(IEnumerable<T> entity);
    public abstract void Delete(IEnumerable<T> entity);
    public abstract void Update(IEnumerable<T> entity);

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
