using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Flurl;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public sealed class ContactServer : OwnedItemServerBase<Contact>, IContactServer
	{
		public ContactServer(
			IConfiguration config,
			string token,
			HttpMessageHandler innerHandler) :
			base(config, token, innerHandler)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(Contact)}";

		public async Task<Contact> ByEmail(string email)
		{
			var convertedEmail = HttpUtility.UrlDecode(email);
			var request = GetRequest(Url.Combine(UriResourceBase, "ByEmail", convertedEmail));
			var retval = await GetResponse<Contact>(request);

			return retval;
		}
	}
}
