using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace GraphML.UI.Uno
{
	using Autofac;
	using GraphML.Common;
	using IdentityModel.Client;
	using Microsoft.Extensions.Configuration;
	using Newtonsoft.Json.Linq;
	using System;
	using System.Net.Http;
	using System.Security.Authentication;
	using Windows.UI.Xaml.Controls;

	public sealed partial class MainPage : Page
	{
		private readonly IConfigurationRoot _config;

		public MainPage()
		{
			this.InitializeComponent();

			_config = App.Container.Resolve<IConfigurationRoot>();
		}

		private async void Login_Click(object sender, object args)
		{
#if __WASM__

			var innerHandler = new global::Uno.UI.Wasm.WasmHttpHandler();
#else
      var innerHandler = new HttpClientHandler
	    {
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      };
#endif
        var client = new HttpClient(innerHandler)
			{
				BaseAddress = new Uri(_config.IDENTITY_SERVER_BASE_URL())
			};
			var disco = await client.GetDiscoveryDocumentAsync();
			if (disco.IsError)
			{
				throw new HttpRequestException(disco.Error);
			}

			var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
			{
				Address = disco.TokenEndpoint,

				ClientId = _config.IDENTITY_SERVER_CLIENT_ID(),
				ClientSecret = _config.IDENTITY_SERVER_CLIENT_SECRET(),

				UserName = _userName.Text,
				Password = _password.Password
			});
			if (response.IsError)
			{
				throw new AuthenticationException(response.Error);
			}

			var api = new HttpClient(innerHandler)
			{
				BaseAddress = new Uri(_config.API_URI())
			};
			api.SetBearerToken(response.AccessToken);

			var orgsResp = await api.GetAsync("api/Organisation/GetAll");
			var orgsCont = await orgsResp.Content.ReadAsStringAsync();
			var orgs = JArray.Parse(orgsCont);

			var rolesResp = await api.GetAsync("api/Role/GetAll");
			var rolesCont = await rolesResp.Content.ReadAsStringAsync();
			var roles = JArray.Parse(rolesCont);
		}
	}
}
