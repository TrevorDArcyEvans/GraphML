using System.Web;
using Flurl;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class ContactServer : OwnedItemServerBase<Contact>, IContactServer
  {
    public ContactServer(
      IRestClientFactory clientFactory,
      ILogger<ContactServer> logger,
      ISyncPolicyFactory policy) :
      base(clientFactory, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = "/api/Contact";

    public Contact ByEmail(string email)
    {
      var convertedEmail = HttpUtility.UrlDecode(email);
      var request = GetRequest(Url.Combine(ResourceBase, "ByEmail", convertedEmail));
      var retval = GetResponse<Contact>(request);

      return retval;
    }
  }
}
