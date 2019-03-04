using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
    private readonly IServer _server;
    private readonly IDatabase _db;

    public ResultDatastore(
      IConfiguration config,
      ILogger<ResultDatastore> logger,
      ISyncPolicyFactory policy)
    {
      _config = config;
      _logger = logger;
      _policy = policy.Build(_logger);

      var dataStore = Settings.RESULT_DATASTORE(_config);
      var cfg = new ConfigurationOptions
      {
        EndPoints =
        {
          { dataStore }
        },
        SyncTimeout = int.MaxValue
      };
      var redis = ConnectionMultiplexer.Connect(cfg);
      _server = redis.GetServer(dataStore);
      _db = redis.GetDatabase();
    }

    public void Create(IRequest request, string resultJson)
    {
      GetInternal(() =>
      {
        // store CorrelationId --> Result
        _db.StringSet(request.CorrelationId, resultJson, Expiry);

        // store ContactId|CorrelationId --> Request
        var jsonReq = JsonConvert.SerializeObject(request);
        _db.StringSet($"{request.ContactId}|{request.CorrelationId}", jsonReq, Expiry);

        return 0;
      });
    }

    public void Delete(string correlationId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IRequest> List(string contactId)
    {
      var keys = _server.Keys();
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
