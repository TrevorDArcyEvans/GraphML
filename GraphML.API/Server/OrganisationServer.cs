using Flurl;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;

namespace GraphML.API.Server
{
  public sealed class OrganisationServer : ItemServerBase<Organisation>, IOrganisationServer
  {
    public OrganisationServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<OrganisationServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(Organisation)}";

    public async Task<IEnumerable<Organisation>> GetAll(int pageIndex, int pageSize)
    {
      var request = GetPageRequest(Url.Combine(ResourceBase, $"{nameof(OrganisationController.GetAll)}"), pageIndex, pageSize);
      var retval = await GetResponse<IEnumerable<Organisation>>(request);

      return retval;
    }
  }
}
