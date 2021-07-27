using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Common;
using GraphML.Interfaces;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphML.API.Server
{
  public sealed class ResultServer : ServerBase, IResultServer
  {
    private readonly JsonSerializerSettings _settings = new()
    {
      Converters = new List<JsonConverter>(
        new JsonConverter[]
        {
          new LookupSerializer<string[]>(),
          new FindDuplicatesResultSerializer()
        })
    };

    public ResultServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<ResultServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Result";

    public async Task Delete(Guid correlationId)
    {
      var request = DeleteRequest(Url.Combine(ResourceBase, nameof(ResultController.Delete), correlationId.ToString()));
      var retval = await RetrieveResponse<object>(request);
    }

    public async Task<IEnumerable<IRequest>> ByContact(Guid contactId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByContact), contactId.ToString()));
      var objs = await GetRequests(request);

      return objs;
    }

    public async Task<IEnumerable<IRequest>> ByOrganisation(Guid orgId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByOrganisation), orgId.ToString()));
      var objs = await GetRequests(request);

      return objs;
    }

    public async Task<IEnumerable<IRequest>> ByGraph(Guid graphId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByGraph), graphId.ToString()));
      var objs = await GetRequests(request);

      return objs;
    }

    public async Task<IRequest> ByCorrelation(Guid correlationId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByCorrelation), correlationId.ToString()));
      var obj = await GetObject(request);

      return (IRequest) obj;
    }

    public async Task<IResult> Retrieve(Guid correlationId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.Retrieve), correlationId.ToString()));
      var obj = await GetObject(request);

      return (IResult) obj;
    }

    private async Task<object> GetObject(HttpRequestMessage request)
    {
      var resp = await RetrieveRawResponse(request);
      await using var strm = await resp.Content.ReadAsStreamAsync();
      using var sr = new StreamReader(strm);
      var json = await sr.ReadToEndAsync();
      var jobj = JObject.Parse(json);
      var reqTypeStr = jobj["type"].ToString();
      var reqType = Type.GetType(reqTypeStr);
      var obj = JsonConvert.DeserializeObject(json, reqType, _settings);
      return obj;
    }

    private async Task<List<IRequest>> GetRequests(HttpRequestMessage request)
    {
      var retval = await RetrieveResponse<IEnumerable<object>>(request);
      var objs = new List<IRequest>();

      foreach (JObject jobj in retval)
      {
        var reqTypeStr = jobj["type"].ToString();
        var reqType = Type.GetType(reqTypeStr);
        var obj = (IRequest) JsonConvert.DeserializeObject(jobj.ToString(), reqType);

        objs.Add(obj);
      }

      return objs;
    }
  }
}
