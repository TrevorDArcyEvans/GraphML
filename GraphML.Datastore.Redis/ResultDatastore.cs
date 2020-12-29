using GraphML.Common;
using GraphML.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Datastore.Redis
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

      var dataStore = _config.RESULT_DATASTORE();
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
        _db.StringSet($"{request.CorrelationId}", resultJson, Expiry);

        // store ContactId|OrganisationId|CorrelationId --> Request
        var jsonReq = JsonConvert.SerializeObject(request);
        _db.StringSet($"{request.Contact.Id}|{request.Contact.OrganisationId}|{request.CorrelationId}", jsonReq, Expiry);

        return 0;
      });
    }

    public void Delete(Guid correlationId)
    {
      GetInternal(() =>
      {
        var keys = _server.Keys()
          .Where(x =>
            x.ToString().StartsWith($"{correlationId}") ||
            x.ToString().EndsWith($"{correlationId}"))
          .ToArray();

        _db.KeyDelete(keys);

        return 0;
      });
    }

    public IEnumerable<IRequest> ByContact(Guid contactId)
    {
      return GetInternal(() =>
      {
        var keys = _server.Keys()
          .Where(x => x.ToString().StartsWith($"{contactId}"))
          .ToArray();
        var reqs = _db.StringGet(keys);
        var retval = new List<IRequest>();
        foreach (var req in reqs)
        {
          var json = req.ToString();
          var jobj = JObject.Parse(json);
          var reqTypeStr = jobj["Type"].ToString();
          var reqType = Type.GetType(reqTypeStr);
          var request = (IRequest)JsonConvert.DeserializeObject(json, reqType);

          retval.Add(request);
        }

        return retval;
      });
    }

    public IEnumerable<IRequest> ByOrganisation(Guid orgId)
    {
      return GetInternal(() =>
      {
        var keys = _server.Keys()
          .Where(x => x.ToString().Contains($"{orgId}"))
          .ToArray();
        var reqs = _db.StringGet(keys);
        var retval = new List<IRequest>();
        foreach (var req in reqs)
        {
          var json = req.ToString();
          var jobj = JObject.Parse(json);
          var reqTypeStr = jobj["Type"].ToString();
          var reqType = Type.GetType(reqTypeStr);
          var request = (IRequest)JsonConvert.DeserializeObject(json, reqType);

          retval.Add(request);
        }

        return retval;
      });
    }

    public IResult Retrieve(Guid correlationId)
    {
      return GetInternal(() =>
      {
        if (!_db.KeyExists($"{correlationId}"))
        {
          return null;
        }

        var json = _db.StringGet($"{correlationId}");
        var jobj = JObject.Parse(json);
        var resTypeStr = jobj["Type"].ToString();
        var resType = Type.GetType(resTypeStr);
        var result = (IResult)JsonConvert.DeserializeObject(json, resType);

        return result;
      });
    }

    private TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }
  }
}
