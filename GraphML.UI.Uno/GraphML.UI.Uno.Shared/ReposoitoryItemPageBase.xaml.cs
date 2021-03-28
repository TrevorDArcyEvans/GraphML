namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

	public abstract partial class ReposoitoryItemPageBase : PageBase
	{
		protected NodeServer NodeServer;
		protected EdgeServer EdgeServer;

		public ReposoitoryItemPageBase()
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

		protected abstract Task<IEnumerable<RepositoryItem>> GetRepositoryItems(Repository repo, int pageIndex, int pageSize);

		private async void Initialise(Repository repo)
		{
			EdgeServer = new EdgeServer(_config, _navArgs.Token, _innerHandler);
			NodeServer = new NodeServer(_config, _navArgs.Token, _innerHandler);

			var repoItems = await GetRepositoryItems(repo, _pageIndex, PageSize);
			repoItems.ToList()
		  .ForEach(repoItem => MarshallToUI(() => RepositoryItems.Add(repoItem)));
		}

		protected void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}
	}
}
