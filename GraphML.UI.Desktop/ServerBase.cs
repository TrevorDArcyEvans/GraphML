﻿using Flurl;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace GraphML.UI.Desktop
{
  public abstract class ServerBase<T>
  {
    private readonly IRestClient _client;
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings();
    protected readonly ILogger<ServerBase<T>> _logger;
    private readonly ISyncPolicy _policy;

    /*

    public DatastoreBase(
      IRestClientFactory crmFactory,
      ILogger<DatastoreBase<T>> logger,
      ISyncPolicyFactory policy,
      IConfiguration config)
    {
      _crmFactory = crmFactory;
      _logCRM = Settings.LOG_CRM(config);
      _logger = logger;
      _policy = policy.Build(_logger);

      _settings.Converters.Add(
        new StringEnumConverter
        {
          CamelCaseText = false
        });
    }

    */
    protected abstract string ResourceBase { get; }

    public ServerBase(IRestClient client)
    {
      _client = client;
    }

    public IEnumerable<T> ByIds(IEnumerable<string> ids)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, "ByIds"), ids);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> ByOwners(IEnumerable<string> ownerIds)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, "ByOwners"), ownerIds);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Create(IEnumerable<T> entity)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, "Create"), entity);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Delete(IEnumerable<T> entity)
    {
      var request = GetDeleteRequest(Url.Combine(ResourceBase, "Delete"), entity);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Update(IEnumerable<T> entity)
    {
      var request = GetPutRequest(Url.Combine(ResourceBase, "Update"), entity);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    protected IRestRequest GetRequest(string path)
    {
      var request = new RestRequest(path)
      {
        Method = Method.GET
      };
      request.AddHeader("Content-Type", "application/json");

      return request;
    }

    protected IRestRequest GetRequest(string path, object body)
    {
      var request = GetRequest(path);
      request.AddJsonBody(body);

      return request;
    }

    protected IRestRequest GetAllRequest(string path)
    {
      var request = GetRequest(path);
      AddGetAllParameters(request);

      return request;
    }

    protected IRestRequest GetAllRequest(string path, object body)
    {
      var request = GetAllRequest(path);
      request.AddJsonBody(body);

      return request;
    }

    protected IRestRequest GetPostRequest(string path, object body)
    {
      var request = GetRequest(path, body);
      request.Method = Method.POST;

      return request;
    }

    protected IRestRequest GetPutRequest(string path, object body)
    {
      var request = GetRequest(path, body);
      request.Method = Method.PUT;

      return request;
    }

    protected IRestRequest GetDeleteRequest(string path, object body)
    {
      var request = GetRequest(path, body);
      request.Method = Method.DELETE;

      return request;
    }

    private static void AddGetAllParameters(IRestRequest request)
    {
      const int StartPageIndex = 1;
      const int GetAllPageSize = int.MaxValue;

      request.AddQueryParameter("PageIndex", StartPageIndex.ToString(CultureInfo.InvariantCulture));
      request.AddQueryParameter("PageSize", GetAllPageSize.ToString(CultureInfo.InvariantCulture));
    }

    protected TOther GetResponse<TOther>(IRestRequest request)
    {
      var resp = GetRawResponse(request);
      var retval = JsonConvert.DeserializeObject<TOther>(resp.Content, _settings);

      return retval;
    }

    protected IRestResponse GetRawResponse(IRestRequest request)
    {
      var resp = _client.Execute(request);

      // log here as may fail deserialisation
      var body = request.Parameters.SingleOrDefault(x => x.Type == ParameterType.RequestBody)?.Value?.ToString();
      var msg = $"[{request.Resource}] {body} --> [{resp.StatusCode}] {resp.Content}";
      LogInformation(msg);

      // relog errors explicitly so they are more prominent
      if (resp.StatusCode >= HttpStatusCode.BadRequest)
      {
        _logger.LogError(msg);
        throw new HttpResponseException(resp.StatusCode, resp.Content);
      }

      return resp;
    }

    protected TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }

    private void LogInformation(string msg)
    {
      _logger.LogInformation(msg);
    }
  }

  public sealed class RepositoryManagerServer : ServerBase<RepositoryManager>
  {
    public RepositoryManagerServer(IRestClient client) :
      base(client)
    {
    }

    protected override string ResourceBase { get; } = "/api/RepositoryManager";

    public IEnumerable<RepositoryManager> GetAll()
    {
      var request = GetAllRequest(Url.Combine(ResourceBase, "GetAll"));
      var retval = GetResponse<IEnumerable<RepositoryManager>>(request);

      return retval;
    }
  }

  public sealed class RepositoryServer : ServerBase<Repository>
  {
    public RepositoryServer(IRestClient client) :
      base(client)
    {
    }

    protected override string ResourceBase { get; } = "/api/Repository";
  }

  public sealed class HttpResponseException : Exception
  {
    public HttpStatusCode StatusCode { get; }

    public HttpResponseException(HttpStatusCode statusCode, string message) :
      base(message)
    {
      StatusCode = statusCode;
    }
  }

  public interface ISyncPolicyFactory
  {
    ISyncPolicy Build(ILogger logger);
  }

  public sealed class SyncPolicyFactory : ISyncPolicyFactory
  {
    public ISyncPolicy Build(ILogger logger)
    {
      return Policy.Handle<Exception>()
        .WaitAndRetry(3, // We can also do this with WaitAndRetryForever... but chose WaitAndRetry this time.
          attempt => TimeSpan.FromSeconds(0.1 * Math.Pow(2, attempt)), // Back off!  2, 4, 8, 16 etc times 1/4-second
            (exception, calculatedWaitDuration) =>  // Capture some info for logging!
            {
              logger.LogError($"Error in {logger.ToString()} after {calculatedWaitDuration.TotalSeconds.ToString()}: {exception.Message}");
            });
    }
  }
}
