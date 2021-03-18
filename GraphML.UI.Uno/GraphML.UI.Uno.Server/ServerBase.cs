using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GraphML.API.Server;
using GraphML.Common;
using GraphML.Interfaces.Server;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Uno.Extensions;

namespace GraphML.UI.Uno.Server
{
	public abstract class ServerBase : IServerBase
	{
		private readonly string _token;
		private readonly HttpClient _client;
		private readonly JsonSerializerSettings _settings = new JsonSerializerSettings();
		private readonly ISyncPolicy _policy;

		protected string UriBase { get; }
		protected abstract string ResourceBase { get; }

		public ServerBase(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler,
			ISyncPolicyFactory policy)
		{
			_token = token;
			_client = new HttpClient(innerHandler);
			_client.SetBearerToken(_token);
			_client.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
			_policy = policy.Build(this.Log());

			UriBase = config.API_URI();
		}

		protected HttpRequestMessage GetRequest(string path)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, path);

			return request;
		}

		protected HttpRequestMessage GetRequest(string path, object body)
		{
			var request = GetRequest(path);
			request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

			return request;
		}

		protected HttpRequestMessage GetAllRequest(string path)
		{
			var request = GetRequest(path);
			AddGetAllParameters(request);

			return request;
		}

		protected HttpRequestMessage GetAllRequest(string path, object body)
		{
			var request = GetAllRequest(path);
			request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

			return request;
		}

		protected HttpRequestMessage GetPostRequest(string path, object body)
		{
			var request = GetRequest(path, body);
			request.Method = HttpMethod.Post;

			return request;
		}

		protected HttpRequestMessage GetPutRequest(string path, object body)
		{
			var request = GetRequest(path, body);
			request.Method = HttpMethod.Put;

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
			request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

			return request;
		}

		protected async Task<TOther> GetResponse<TOther>(HttpRequestMessage request)
		{
			var resp = await GetRawResponse(request);
			var content = await resp.Content.ReadAsStringAsync();
			var retval = JsonConvert.DeserializeObject<TOther>(content, _settings);

			return retval;
		}

		protected async Task<HttpResponseMessage> GetRawResponse(HttpRequestMessage request)
		{
			var resp = await _client.SendAsync(request);

			// log here as may fail deserialisation
			this.Log().LogInformation(request.ToString());

			// relog errors explicitly so they are more prominent
			if (resp.StatusCode >= HttpStatusCode.BadRequest)
			{
				this.Log().LogError(resp.ToString());
#if __WASM__
				throw new Exception($"HttpResponseException: {resp.StatusCode} --> {resp.Content}");
#else
				throw new HttpResponseException(resp.StatusCode);
#endif
			}

			return resp;
		}

		protected TOther GetInternal<TOther>(Func<TOther> get)
		{
			return _policy.Execute(get);
		}

		private static void AddGetAllParameters(HttpRequestMessage request)
		{
			const int StartPageIndex = 1;
			const int GetAllPageSize = int.MaxValue;

			var builder = new UriBuilder(request.RequestUri);
			var query = HttpUtility.ParseQueryString(builder.Query);

			query["PageIndex"] = StartPageIndex.ToString(CultureInfo.InvariantCulture);
			query["PageSize"] = GetAllPageSize.ToString(CultureInfo.InvariantCulture);

			builder.Query = query.ToString();
			request.RequestUri = new Uri(builder.ToString());
		}
	}
}
