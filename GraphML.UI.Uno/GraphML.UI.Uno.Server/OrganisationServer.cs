using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GraphML.Interfaces.Server;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace GraphML.UI.Uno.Server
{
	public sealed class OrganisationServer : IOrganisationServer
	{
		private readonly HttpClient _api;

		public OrganisationServer(
			HttpMessageHandler innerHandler,
	  Uri baseUri,
			string token)
		{
			_api = new HttpClient(innerHandler)
			{
				BaseAddress = baseUri
			};
			_api.SetBearerToken(token);
		}

		public Task<IEnumerable<Organisation>> ByIds(IEnumerable<Guid> ids)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Organisation>> Create(IEnumerable<Organisation> entity)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Organisation>> Delete(IEnumerable<Organisation> entity)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Organisation>> GetAll()
		{
			var orgsResp = await _api.GetAsync("api/Organisation/GetAll");
			var orgsCont = await orgsResp.Content.ReadAsStringAsync();
			var orgs = JsonConvert.DeserializeObject<List<Organisation>>(orgsCont);
			return orgs;
		}

		public Task<IEnumerable<Organisation>> Update(IEnumerable<Organisation> entity)
		{
			throw new NotImplementedException();
		}
	}
}
