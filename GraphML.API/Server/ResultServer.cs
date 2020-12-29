using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class ResultServer : ServerBase, IResultServer
  {
    public ResultServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<ResultServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Result";

    public async Task Delete(string correlationId)
    {
      var request = GetDeleteRequest(Url.Combine(ResourceBase, nameof(ResultController.Delete), correlationId));
      var retval = await GetResponse<object>(request);
    }

    public async Task<IEnumerable<IRequest>> ByContact(string contactId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByContact), contactId));
      var retval = await GetResponse<IEnumerable<IRequest>>(request);

      return retval;
    }

    public async Task<IEnumerable<IRequest>> ByOrganisation(string orgId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.ByOrganisation), orgId));
      var retval = await GetResponse<IEnumerable<IRequest>>(request);

      return retval;
    }

    public async Task<IResult> Retrieve(string correlationId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, nameof(ResultController.Retrieve), correlationId));
      var retval = await GetResponse<IResult>(request);

      return retval;
    }
  }
}
