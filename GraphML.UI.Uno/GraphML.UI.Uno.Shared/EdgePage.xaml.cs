namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class EdgePage : PageBase
	{
		public EdgePage()
		{
			InitializeComponent();
		}

		public ObservableCollection<RepositoryItem> RepositoryItems { get; set; } = new ObservableCollection<RepositoryItem>();

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedRepository);
		}

		private async void Initialise(Repository repo)
		{
			var repoItemsServer = new EdgeServer(_config, _navArgs.Token, _innerHandler);
			var repoItems = await repoItemsServer.ByOwners(new[] { repo.Id });
			repoItems.ToList()
		  .ForEach(repoItem => MarshallToUI(() => RepositoryItems.Add(repoItem)));
		}

    private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}
	}
}
