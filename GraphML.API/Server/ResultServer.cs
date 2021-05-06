using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
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

    public async Task<IEnumerable<IRequest>> ByOrganisation(Guid orgId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByOrganisation), orgId.ToString()));
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

    public async Task<IEnumerable<IRequest>> ByGraph(Guid graphId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByGraph), graphId.ToString()));
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

    public async Task<IRequest> ByCorrelation(Guid corrId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByCorrelation), corrId.ToString()));
      var retval = await RetrieveResponse<object>(request);
      var jobj = (JObject) retval;
      var reqTypeStr = jobj["type"].ToString();
      var reqType = Type.GetType(reqTypeStr);
      var obj = JsonConvert.DeserializeObject(jobj.ToString(), reqType);

      return (IRequest) obj;
    }

    public async Task<IResult> Retrieve(Guid correlationId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.Retrieve), correlationId.ToString()));
      var retval = await RetrieveResponse<object>(request);
      var jobj = (JObject) retval;
      var resTypeStr = jobj["type"].ToString();
      var resType = Type.GetType(resTypeStr);
      var obj = JsonConvert.DeserializeObject(jobj.ToString(), resType);

      return (IResult) obj;
    }
  }
}
