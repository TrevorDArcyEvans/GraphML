using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Common;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GraphML.API.Server
{
  public sealed class IdentityServer : ServerBase, IIdentityServer
  {
    public IdentityServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<IdentityServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/Identity";

    public async Task<LookupEx<string, string>> GetAPIUserClaims()
    {
      var request = GetRequest(Url.Combine(ResourceBase, $"{nameof(IdentityController.GetAPIUserClaims)}"));

      // have to get raw response as does not JSON deserialise
      var resp = await RetrieveRawResponse(request);
      var json = await resp.Content.ReadAsStringAsync();
      var retval = JsonConvert.DeserializeObject<LookupEx<string, string>>(json, new LookupSerializer<string>());

      return retval;
    }
  }
}
