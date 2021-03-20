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
using Newtonsoft.Json;

namespace GraphML.UI.Uno.Shared
{
	public sealed partial class RepositoryManagerPage : Page
	{
		private readonly IConfigurationRoot _config;
		private string _token;

		public RepositoryManagerPage()
		{
			InitializeComponent();

			_config = App.Container.Resolve<IConfigurationRoot>();
		}

		public ObservableCollection<RepositoryManager> RepositoryManagers { get; set; } = new ObservableCollection<RepositoryManager>();
		public RepositoryManager SelectedRepositoryManager { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			var navArgs = JsonConvert.DeserializeObject<Dictionary<string, string>>((string)e.Parameter);
			_token = navArgs["Token"];
			var selOrg = JsonConvert.DeserializeObject<Organisation>(navArgs["SelectedOrganisation"]);

      SelectedOrganisation.Text = $"SelectedOrganisation:  " + (string)e.Parameter;//selOrg.Name;

			base.OnNavigatedTo(e);

			Initialise(selOrg);
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

		private void Back_Click(object sender, object args)
		{
			var navArgs = new Dictionary<string, object>
	  {
		  { "Token", _token }
	  };
			Frame.Navigate(typeof(OrganisationPage), navArgs);
		}
	}
}
