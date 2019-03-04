using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphML.ResultDatastore.Redis
{
  public sealed class ResultDatastore : IResultDatastore
  {
    private static TimeSpan Expiry = TimeSpan.FromDays(28);

    private readonly IConfiguration _config;
    private readonly ILogger<ResultDatastore> _logger;
    private readonly ISyncPolicy _policy;
    private readonly IDatabase _db;

    public ResultDatastore(
      IConfiguration config,
      ILogger<ResultDatastore> logger,
      ISyncPolicyFactory policy)
    {
      _config = config;
      _logger = logger;
      _policy = policy.Build(_logger);

      var cacheHost = Settings.RESULT_DATASTORE(_config);
      var cfg = new ConfigurationOptions
      {
        EndPoints =
        {
          { cacheHost }
        },
        SyncTimeout = int.MaxValue
      };
      var redis = ConnectionMultiplexer.Connect(cfg);
      _db = redis.GetDatabase();
    }

    public void Create(IRequest request, string resultJson)
    {
      GetInternal(() =>
      {
        _db.StringSet(request.CorrelationId, resultJson, Expiry);
        return 0;
      });
    }

    public void Delete(string correlationId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IRequest> List(string contactId)
    {
      throw new NotImplementedException();
    }

    public string Retrieve(string correlationId)
    {
      throw new NotImplementedException();
    }

    private TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }
  }
}
