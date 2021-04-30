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
      var retval = await RetrieveResponse<IEnumerable<IRequest>>(request);

      return retval;
    }

    public async Task<IEnumerable<IRequest>> ByOrganisation(Guid orgId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByOrganisation), orgId.ToString()));
      var retval = await RetrieveResponse<IEnumerable<IRequest>>(request);

      return retval;
    }

    public async Task<IRequest> ByCorrelation(Guid corrId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByCorrelation), corrId.ToString()));
      var retval = await RetrieveResponse<IRequest>(request);

      return retval;
    }

    public async Task<IResult> Retrieve(Guid correlationId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.Retrieve), correlationId.ToString()));
      var retval = await RetrieveResponse<IResult>(request);

      return retval;
    }
  }
}
