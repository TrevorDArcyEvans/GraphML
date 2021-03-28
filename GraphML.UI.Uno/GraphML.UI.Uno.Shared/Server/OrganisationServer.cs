using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using GraphML.Interfaces.Server;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Uno.Server
{
	public sealed class OrganisationServer : ItemServerBase<Organisation>, IOrganisationServer
	{
		public OrganisationServer(
	  IConfiguration config,
			string token,
	  HttpMessageHandler innerHandler) :
	  base(config, token, innerHandler)
		{
		}

		protected override string ResourceBase { get; } = $"/api/{nameof(Organisation)}";

    public async Task<IEnumerable<Organisation>> GetAll(int pageIndex, int pageSize)
    {			
      var request = GetPageRequest(Url.Combine(UriResourceBase, "GetAll"), pageIndex, pageSize);
			var retval = await GetResponse<IEnumerable<Organisation>>(request);

			return retval;
    }
  }
}
