using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Autofac;
using GraphML.Common;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace GraphML.UI.Uno
{
	public sealed partial class MainPage : Page
	{
		private readonly IConfigurationRoot _config;
		private string _token;

		public MainPage()
		{
			InitializeComponent();

			_config = App.Container.Resolve<IConfigurationRoot>();
		}

		public ObservableCollection<string> Organisations { get; set; } = new ObservableCollection<string>();
		public string SelectedOrganisation { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e.Parameter is string token &&
				!string.IsNullOrWhiteSpace(token))
			{
				_token = token;
			}
			else
			{
				throw new ArgumentNullException("null token");
			}
			base.OnNavigatedTo(e);

			Initialise();
		}

		private async void Initialise()
		{
#if __WASM__
			var innerHandler = new global::Uno.UI.Wasm.WasmHttpHandler();
#else
			var innerHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};
#endif

			var api = new HttpClient(innerHandler)
			{
				BaseAddress = new Uri(_config.API_URI())
			};
			api.SetBearerToken(_token);

			var orgsResp = await api.GetAsync("api/Organisation/GetAll");
			var orgsCont = await orgsResp.Content.ReadAsStringAsync();
			var orgs = JArray.Parse(orgsCont).ToList();
			orgs.ForEach(org => Organisations.Add(org["Name"].ToString()));

			var rolesResp = await api.GetAsync("api/Role/GetAll");
			var rolesCont = await rolesResp.Content.ReadAsStringAsync();
			var roles = JArray.Parse(rolesCont);
		}

		private void Logout_Click(object sender, object args)
		{
			Frame.Navigate(typeof(LoginPage));
		}
	}
}
