using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public sealed class OrganisationServer : ItemServerBase<Organisation>, IOrganisationServer
  {
    public OrganisationServer(
      IRestClientFactory clientFactory,
      ILogger<OrganisationServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Organisation";

    public async Task<IEnumerable<Organisation>> GetAll()
    {
      var request = GetAllRequest(Url.Combine(ResourceBase, "GetAll"));
      var retval = await GetResponse<IEnumerable<Organisation>>(request);

      return retval;
    }
  }
}
