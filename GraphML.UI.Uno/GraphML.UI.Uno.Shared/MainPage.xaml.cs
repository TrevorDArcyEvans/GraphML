namespace GraphML.UI.Uno
{
	using Autofac;
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
			var innerHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};
			var client = new HttpClient(innerHandler)
			{
				// TODO	GraphML.Common.Settings
				BaseAddress = new Uri(_config["Identity_Server:Base_Url"])
			};
			var disco = await client.GetDiscoveryDocumentAsync();
			if (disco.IsError)
			{
				throw new HttpRequestException(disco.Error);
			}

			var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
			{
				Address = disco.TokenEndpoint,

				// TODO	GraphML.Common.Settings
				ClientId = _config["Identity_Server:Client_Id"],
				ClientSecret = _config["Identity_Server:Client_Secret"],

				UserName = _userName.Text,
				Password = _password.Password
			});
			if (response.IsError)
			{
				throw new AuthenticationException(response.Error);
			}


			var api = new HttpClient(innerHandler)
			{
				// TODO	GraphML.Common.Settings
				BaseAddress = new Uri(_config["API:Uri"])
			};
			var token = response.AccessToken;
			api.SetBearerToken(token);
			var orgsResp = await api.GetAsync("api/Organisation/GetAll");
			var orgsCont = await orgsResp.Content.ReadAsStringAsync();
			var orgs = JArray.Parse(orgsCont);
		}
	}
}
