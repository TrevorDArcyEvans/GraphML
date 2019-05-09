using Microsoft.Extensions.Logging;

namespace GraphML.UI.Desktop
{
  public sealed class OrganisationServer : ServerBase<Organisation>, IOrganisationServer
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
