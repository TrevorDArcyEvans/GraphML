namespace GraphML.UI.Uno
{
	using System.Collections.ObjectModel;
	using System.Linq;
#if !__WASM__
	using System.Net.Http;
#endif
	using Windows.UI.Xaml.Navigation;
	using GraphML.UI.Uno.Server;

	public sealed partial class RepositoryPage : PageBase
	{
		public RepositoryPage() :
			base()
		{
			this.InitializeComponent();
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
			var repoServer = new RepositoryServer(_config, _navArgs.Token, _innerHandler);
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
