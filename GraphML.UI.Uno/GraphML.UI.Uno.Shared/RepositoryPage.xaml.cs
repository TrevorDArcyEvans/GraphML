namespace GraphML.UI.Uno.Shared
{
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

	public sealed partial class RepositoryPage : Page
	{
		private readonly IConfigurationRoot _config;
    private BreadcrumbTrail _navArgs = new BreadcrumbTrail();

		public RepositoryPage()
		{
			this.InitializeComponent();

			_config = App.Container.Resolve<IConfigurationRoot>();
		}

		public ObservableCollection<Repository> Repositories { get; set; } = new ObservableCollection<Repository>();
		public Repository SelectedRepository { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedRepositoryManager);
		}

		private async void Initialise(RepositoryManager repoMgr)
		{
#if __WASM__
			var innerHandler = new global::Uno.UI.Wasm.WasmHttpHandler();
#else
			var innerHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
			};
#endif

			var repoServer = new RepositoryServer(_config, _navArgs.Token, innerHandler);
			var repos = await repoServer.ByOwners(new[] { repoMgr.Id });
			repos.ToList()
		  .ForEach(repo => Repositories.Add(repo));
		}

		private void Repository_Click(object sender, object args)
		{
			_navArgs.SelectedRepository = SelectedRepository;
			// TODO	  Frame.Navigate(typeof(GraphPage), navArgs);
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryManagerPage), _navArgs);
		}
	}
}
