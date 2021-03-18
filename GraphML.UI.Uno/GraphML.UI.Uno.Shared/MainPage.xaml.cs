using System;
using System.Collections.ObjectModel;
using System.Linq;
#if !__WASM__
using System.Net.Http;
#endif
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Autofac;
using GraphML.API.Server;
using GraphML.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GraphML.UI.Uno.Server;
using Uno.Extensions;

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

		public ObservableCollection<Organisation> Organisations { get; set; } = new ObservableCollection<Organisation>();
		public Organisation SelectedOrganisation { get; set; }

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

			var spf = new SyncPolicyFactory();
			var orgServer = new OrganisationServer(_config, _token, innerHandler, spf);
			var orgs = await orgServer.GetAll();
			orgs.ToList()
		  .ForEach(org => Organisations.Add(org));

			//var rolesResp = await api.GetAsync("api/Role/GetAll");
			//var rolesCont = await rolesResp.Content.ReadAsStringAsync();
			//var roles = JArray.Parse(rolesCont);
		}

		private void Logout_Click(object sender, object args)
		{
			Frame.Navigate(typeof(LoginPage));
		}
	}
}
