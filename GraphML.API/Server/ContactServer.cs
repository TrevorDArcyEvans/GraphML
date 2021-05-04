using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Flurl;
using GraphML.API.Controllers;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GraphML.API.Server
{
  public sealed class ContactServer : OwnedItemServerBase<Contact>, IContactServer
  {
    public ContactServer(
      IConfiguration config,
      IHttpContextAccessor httpContextAccessor,
      HttpClient client,
      ILogger<ContactServer> logger,
      ISyncPolicyFactory policy) :
      base(config, httpContextAccessor, client, logger, policy)
    {
    }

    protected override string ResourceBase { get; } = $"/api/{nameof(Contact)}";

    public async Task<Contact> ByEmail(string email)
    {
      var convertedEmail = HttpUtility.UrlDecode(email);
      var request = GetRequest(Url.Combine(ResourceBase, $"{nameof(ContactController.ByEmail)}", convertedEmail));
      var retval = await RetrieveResponse<Contact>(request);

      return retval;
    }
  }
}
