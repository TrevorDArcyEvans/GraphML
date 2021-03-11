using System.Net.Http;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace GraphML.UI.Uno
{
	using System;
	using Windows.UI.Xaml.Controls;

	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();

			dp.Date = new DateTime(2006, 2, 20);
		}

		private async void OnClick(object sender, object args)
		{
			var dt = DateTime.Now.ToString("O");
			txt.Text = dt;

			var innerHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};
			var client = new HttpClient(innerHandler)
			{
				BaseAddress = new Uri("https://localhost:44387")
			};
			var disco = await client.GetDiscoveryDocumentAsync();
			if (disco.IsError)
			{
				throw new Exception(disco.Error);
			}

			var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
			{
				Address = disco.TokenEndpoint,

				ClientId = "GraphML.UI.Uno",
				ClientSecret = "secret",

				UserName = "alice",
				Password = "Pass123$"
			});

			var api = new HttpClient(innerHandler)
			{
				BaseAddress = new Uri("https://localhost:5001/")
			};
			var token = response.AccessToken;
			api.SetBearerToken(token);
			var orgsResp = await api.GetAsync("api/Organisation/GetAll");
			var orgsCont = await orgsResp.Content.ReadAsStringAsync();
			var orgs = JArray.Parse(orgsCont);
		}

		public void Menu_OnClick(object sender, object args)
		{
			txt.Text = "Reset";
		}
	}
}
