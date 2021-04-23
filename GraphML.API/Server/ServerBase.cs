using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraphML.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Polly;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace GraphML.API.Server
{
  public abstract class ServerBase
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _client;
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings();
    private readonly ILogger<ServerBase> _logger;
    private readonly ISyncPolicy _policy;

    protected abstract string ResourceBase { get; }

    public ServerBase(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<ServerBase> logger,
      ISyncPolicyFactory policy)
    {
      _httpContextAccessor = httpContextAccessor;
      _client = client;
      _logger = logger;
      _policy = policy.Build(_logger);

      _client.BaseAddress = new Uri(config.API_URI());
    }

    protected HttpRequestMessage GetRequest(string path)
    {
      var request = new HttpRequestMessage()
      {
        RequestUri = new Uri(_client.BaseAddress , path),
        Method = HttpMethod.Get
      };
      var accessToken = _httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result; // TODO async
      if (accessToken != null)
      {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
      }

      return request;
    }

    private HttpRequestMessage GetRequest(string path, object body)
    {
      var request = GetRequest(path);
      var json = JsonConvert.SerializeObject(body, _settings);
      request.Content = new StringContent(json, Encoding.UTF8, "application/json");
      request.Headers.Add("Content-Type", "application/json");

      return request;
    }

    protected HttpRequestMessage GetPageRequest(string path, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetRequest(path);
      AddGetPageParameters(request, pageIndex, pageSize, searchTerm);

      return request;
    }

    private HttpRequestMessage GetPageRequest(string path, object body, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(path, pageIndex, pageSize, searchTerm);
      var json = JsonConvert.SerializeObject(body, _settings);
      request.Content = new StringContent(json, Encoding.UTF8, "application/json");
      request.Headers.Add("Content-Type", "application/json");

      return request;
    }

    protected HttpRequestMessage GetPostRequest(string path, object body)
    {
      var request = GetRequest(path, body);
      request.Method = HttpMethod.Post;
      request.Headers.Add("Content-Type", "application/json");

      return request;
    }

    protected HttpRequestMessage GetPostRequest(string path, object body, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(path, body, pageIndex, pageSize, searchTerm);
      request.Method = HttpMethod.Post;
      request.Headers.Add("Content-Type", "application/json");

      return request;
    }

    protected HttpRequestMessage GetPutRequest(string path, object body)
    {
      var request = GetRequest(path, body);
      request.Method = HttpMethod.Post;
      request.Headers.Add("Content-Type", "application/json");

      return request;
    }

    protected HttpRequestMessage GetDeleteRequest(string path)
    {
      var request = GetRequest(path);
      request.Method = HttpMethod.Delete;

      return request;
    }

    protected HttpRequestMessage GetDeleteRequest(string path, object body)
    {
      var request = GetDeleteRequest(path);
      var json = JsonConvert.SerializeObject(body, _settings);
      request.Content = new StringContent(json, Encoding.UTF8, "application/json");
      request.Headers.Add("Content-Type", "application/json");

      return request;
    }

    protected async Task<TOther> GetResponse<TOther>(HttpRequestMessage request)
    {
      var resp = await GetRawResponse(request);
      var json = await resp.Content.ReadAsStringAsync();
      var retval = JsonConvert.DeserializeObject<TOther>(json, _settings);

      return retval;
    }

    protected async Task<HttpResponseMessage> GetRawResponse(HttpRequestMessage request)
    {
      return await GetInternal(async () =>
      {
        var resp =  _client.SendAsync(request, new CancellationTokenSource().Token).Result;

        resp.EnsureSuccessStatusCode();

        return resp;
      });
    }

    private TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }

    private static void AddGetPageParameters(HttpRequestMessage request, int pageIndex, int pageSize, string searchTerm)
    {
      var queryParams = new Dictionary<string, string>
      {
        { "PageIndex", pageIndex.ToString(CultureInfo.InvariantCulture) },
        { "PageSize", pageSize.ToString(CultureInfo.InvariantCulture) },
        { "searchTerm", searchTerm },
      };
      var newUri = QueryHelpers.AddQueryString(request.RequestUri.ToString(), queryParams);
      request.RequestUri = new Uri(newUri);
    }

    private void LogInformation(string msg)
    {
      _logger.LogInformation(msg);
    }
  }
}
