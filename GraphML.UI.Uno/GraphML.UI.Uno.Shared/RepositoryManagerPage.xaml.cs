namespace GraphML.UI.Uno.Shared
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
	using GraphML.UI.Uno.Server;
	using Microsoft.Extensions.Configuration;

	public sealed partial class RepositoryManagerPage : Page
	{
		private readonly IConfigurationRoot _config;
    private Organisation SelectedOrganisation;
		private string _token;
		private Dictionary<string, object> _navArgs = new Dictionary<string, object>();

		public RepositoryManagerPage()
		{
			InitializeComponent();

			_config = App.Container.Resolve<IConfigurationRoot>();
		}

		public ObservableCollection<RepositoryManager> RepositoryManagers { get; set; } = new ObservableCollection<RepositoryManager>();
		public RepositoryManager SelectedRepositoryManager { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (Dictionary<string, object>)e.Parameter;
			_token = (string)_navArgs["Token"];
      SelectedOrganisation = (Organisation)_navArgs["SelectedOrganisation"];

			base.OnNavigatedTo(e);

			Initialise(SelectedOrganisation);
		}

		private async void Initialise(Organisation selOrg)
		{
#if __WASM__
			var innerHandler = new global::Uno.UI.Wasm.WasmHttpHandler();
#else
			var innerHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};
#endif

			var repoMgrServer = new RepositoryManagerServer(_config, _token, innerHandler);
			var repoMgrs = await repoMgrServer.ByOwners(new[] { selOrg.Id });
			repoMgrs.ToList()
		  .ForEach(repoMgr => RepositoryManagers.Add(repoMgr));
		}

		private void Repository_Click(object sender, object args)
		{
			_navArgs["SelectedRepositoryManager"] = SelectedRepositoryManager;
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(OrganisationPage), _navArgs);
		}
	}
}
