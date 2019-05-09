using Flurl;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace GraphML.UI.Desktop
{
  public abstract class ItemServerBase<T> : IItemServerBase<T>
  {
    private readonly IRestClient _client;
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings();
    protected readonly ILogger<OwnedItemServerBase<T>> _logger;
    private readonly ISyncPolicy _policy;

    protected abstract string ResourceBase { get; }

    public ItemServerBase(
      IRestClientFactory clientFactory,
      ILogger<OwnedItemServerBase<T>> logger,
      ISyncPolicyFactory policy)
    {
      _client = clientFactory.GetRestClient();
      _logger = logger;
      _policy = policy.Build(_logger);
    }

    public IEnumerable<T> ByIds(IEnumerable<string> ids)
    {
      var request = GetPostRequest(Url.Combine(ResourceBase, "ByIds"), ids);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Create(IEnumerable<T> entity)
    {
      var request = GetPostRequest(ResourceBase, entity);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Delete(IEnumerable<T> entity)
    {
      var request = GetDeleteRequest(ResourceBase, entity);
      var retval = GetResponse<IEnumerable<T>>(request);

      return retval;
    }

    public IEnumerable<T> Update(IEnumerable<T> entity)
    {
      var request = GetPutRequest(ResourceBase, entity);
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
}
