namespace GraphML.UI.Uno
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
#if !__WASM__
	using System.Net.Http;
#endif
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;
	using Autofac;
	using Microsoft.Extensions.Configuration;
	using GraphML.UI.Uno.Server;
	using GraphML.UI.Uno.Shared;

	public sealed partial class OrganisationPage : Page
	{
		private readonly IConfigurationRoot _config;
		private string _token;
		private Dictionary<string, object> _navArgs = new Dictionary<string, object>();

		public OrganisationPage()
		{
			InitializeComponent();

			_config = App.Container.Resolve<IConfigurationRoot>();
		}

		public ObservableCollection<Organisation> Organisations { get; set; } = new ObservableCollection<Organisation>();
		public Organisation SelectedOrganisation { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (Dictionary<string, object>)e.Parameter;
			_token = (string)_navArgs["Token"];

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

			var orgServer = new OrganisationServer(_config, _token, innerHandler);
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

		private void Organisation_Click(object sender, object args)
		{
			_navArgs["SelectedOrganisation"] = SelectedOrganisation;
			Frame.Navigate(typeof(RepositoryManagerPage), _navArgs);
		}
	}
}
