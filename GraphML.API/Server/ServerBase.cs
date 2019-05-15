using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public abstract class ServerBase : IServerBase
  {
    private readonly IRestClient _client;
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings();
    protected readonly ILogger<ServerBase> _logger;
    private readonly ISyncPolicy _policy;

    protected abstract string ResourceBase { get; }

    public ServerBase(
      IRestClientFactory clientFactory,
      ILogger<ServerBase> logger,
      ISyncPolicyFactory policy)
    {
      _client = clientFactory.GetRestClient();
      _logger = logger;
      _policy = policy.Build(_logger);
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

    protected IRestRequest GetDeleteRequest(string path)
    {
      var request = GetRequest(path);
      request.Method = Method.DELETE;

      return request;
    }

    protected IRestRequest GetDeleteRequest(string path, object body)
    {
      var request = GetDeleteRequest(path);
      request.AddJsonBody(body);

      return request;
    }

    protected async Task<TOther> GetResponse<TOther>(IRestRequest request)
    {
      var resp = await GetRawResponse(request);
      var retval = JsonConvert.DeserializeObject<TOther>(resp.Content, _settings);

      return retval;
    }

    protected async Task<IRestResponse> GetRawResponse(IRestRequest request)
    {
      var resp = await _client.ExecuteTaskAsync(request, new CancellationTokenSource().Token);

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

    private static void AddGetAllParameters(IRestRequest request)
    {
      const int StartPageIndex = 1;
      const int GetAllPageSize = int.MaxValue;

      request.AddQueryParameter("PageIndex", StartPageIndex.ToString(CultureInfo.InvariantCulture));
      request.AddQueryParameter("PageSize", GetAllPageSize.ToString(CultureInfo.InvariantCulture));
    }

    private void LogInformation(string msg)
    {
      _logger.LogInformation(msg);
    }
  }
}
