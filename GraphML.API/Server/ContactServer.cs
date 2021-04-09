using System.Threading.Tasks;
using System.Web;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class ContactServer : OwnedItemServerBase<Contact>, IContactServer
  {
    public ContactServer(
      IHttpContextAccessor httpContextAccessor,
      IRestClientFactory clientFactory,
      ILogger<ContactServer> logger,
      ISyncPolicyFactory policy) :
      base(httpContextAccessor, clientFactory, logger, policy)
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
