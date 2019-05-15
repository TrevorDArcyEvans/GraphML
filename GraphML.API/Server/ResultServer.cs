using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class ResultServer : ServerBase, IResultServer
  {
    public ResultServer(
      IRestClientFactory clientFactory,
      ILogger<ResultServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Result";

    public async Task Delete(string correlationId)
    {
      var request = GetDeleteRequest(Url.Combine(ResourceBase, "Delete", correlationId));
      var retval = await GetResponse<object>(request);
    }

    public async Task<IEnumerable<IRequest>> List(string contactId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "List", contactId));
      var retval = await GetResponse<IEnumerable<IRequest>>(request);

      return retval;
    }

    public async Task<IResult> Retrieve(string correlationId)
    {
      var request = GetRequest(Url.Combine(ResourceBase, "Retrieve", correlationId));
      var retval = await GetResponse<IResult>(request);

      return retval;
    }
  }
}
