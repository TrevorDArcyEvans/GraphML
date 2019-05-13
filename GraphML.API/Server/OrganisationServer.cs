using Microsoft.Extensions.Logging;

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
  }
}
