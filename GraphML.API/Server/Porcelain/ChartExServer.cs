using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers.Porcelain;
using GraphML.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GraphML.API.Server.Porcelain
{
  public sealed class ChartExServer : OwnedItemServerBase<ChartEx>, IChartExServer
  {
    public ChartExServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<ChartExServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/porcelain/ChartEx";

    public async Task<ChartEx> ById(Guid id)
    {
      var request = GetRequest(Url.Combine(ResourceBase, $"{nameof(ChartExController.ById)}", id.ToString()));
      var response = await GetRawResponse(request);
      var json = await response.Content.ReadAsStringAsync();
      var retval = JsonConvert.DeserializeObject<ChartEx>(json);

      return retval;
    }
  }
}
