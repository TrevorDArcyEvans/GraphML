using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GraphML.API.Server
{
  public abstract class ServerBase
  {
    private static JsonSerializerSettings _settings = new JsonSerializerSettings
    {
      ContractResolver = new CamelCasePropertyNamesContractResolver(),
      Formatting = Formatting.Indented,
      Converters = new List<JsonConverter>(new JsonConverter[]
      {
        new StringEnumConverter(),

        // https://stackoverflow.com/questions/18193281/force-json-net-to-include-milliseconds-when-serializing-datetime-even-if-ms-com
        // https://stackoverflow.com/questions/10286204/what-is-the-right-json-date-format
        new IsoDateTimeConverter
        {
          DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"
        },
        new LookupSerializer<string[]>(),
        new FindDuplicatesResultSerializer()
      })
    };

    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly HttpClient _client;
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
      _config = config;
      _httpContextAccessor = httpContextAccessor;
      _client = client;
      _logger = logger;
      _policy = policy.Build(_logger);

      _client.BaseAddress ??= new Uri(_config.API_URI());
    }

    protected HttpRequestMessage GetRequest(string path)
    {
      var request = new HttpRequestMessage()
      {
        RequestUri = new Uri(_client.BaseAddress, path),
        Method = HttpMethod.Get
      };
      var accessToken = _httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result; // TODO async
      if (accessToken != null)
      {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
      }

      return request;
    }

    protected HttpRequestMessage GetPageRequest(string path, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetRequest(path);
      AddGetPageParameters(request, pageIndex, pageSize, searchTerm);

      return request;
    }

    protected HttpRequestMessage PostRequest(string path, object body)
    {
      var request = CreateRequest(path, body);
      request.Method = HttpMethod.Post;

      return request;
    }

    protected HttpRequestMessage PostPageRequest(string path, object body, int pageIndex, int pageSize, string searchTerm)
    {
      var request = GetPageRequest(path, pageIndex, pageSize, searchTerm);
      var json = JsonConvert.SerializeObject(body, _settings);
      request.Content = new StringContent(json, Encoding.UTF8, "application/json");
      request.Method = HttpMethod.Post;

      return request;
    }

    protected HttpRequestMessage PutRequest(string path, object body)
    {
      var request = CreateRequest(path, body);
      request.Method = HttpMethod.Put;

      return request;
    }

    protected HttpRequestMessage DeleteRequest(string path)
    {
      var request = GetRequest(path);
      request.Method = HttpMethod.Delete;

      return request;
    }

    protected HttpRequestMessage DeleteRequest(string path, object body)
    {
      var request = DeleteRequest(path);
      var json = JsonConvert.SerializeObject(body, _settings);
      request.Content = new StringContent(json, Encoding.UTF8, "application/json");

      return request;
    }

    protected async Task<TOther> RetrieveResponse<TOther>(HttpRequestMessage request)
    {
      var resp = await RetrieveRawResponse(request);
      await using var strm = await resp.Content.ReadAsStreamAsync();
      using var sr = new StreamReader(strm);
      var json = await sr.ReadToEndAsync();
      var retval = JsonConvert.DeserializeObject<TOther>(json, _settings);

      return retval;
    }

    protected async Task<HttpResponseMessage> RetrieveRawResponse(HttpRequestMessage request)
    {
      return await GetInternal(async () =>
      {
        var resp = await _client.SendAsync(request, new CancellationTokenSource().Token);

        resp.EnsureSuccessStatusCode();

        return resp;
      });
    }

    private HttpRequestMessage CreateRequest(string path, object body)
    {
      var request = GetRequest(path);
      var json = JsonConvert.SerializeObject(body, _settings);
      request.Content = new StringContent(json, Encoding.UTF8, "application/json");
      if (body is null)
      {
        request.Content.Headers.Add(@"Content-Length", "0");
      }

      return request;
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
