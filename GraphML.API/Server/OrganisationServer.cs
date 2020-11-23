using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Controllers;

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

    protected override string ResourceBase { get; } = $"/api/{nameof(Organisation)}";

    public async Task<IEnumerable<Organisation>> GetAll()
    {
      var request = GetAllRequest(Url.Combine(ResourceBase, $"{nameof(OrganisationController.GetAll)}"));
      var retval = await GetResponse<IEnumerable<Organisation>>(request);

      return retval;
    }
  }
}
