using System.Threading.Tasks;
using System.Web;
using Flurl;
using GraphML.API.Controllers;
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

    protected override string ResourceBase { get; } = $"/api/{nameof(Contact)}";

    public async Task<Contact> ByEmail(string email)
    {
      var convertedEmail = HttpUtility.UrlDecode(email);
      var request = GetRequest(Url.Combine(ResourceBase, $"{nameof(ContactController.ByEmail)}", convertedEmail));
      var retval = await GetResponse<Contact>(request);

      return retval;
    }
  }
}
