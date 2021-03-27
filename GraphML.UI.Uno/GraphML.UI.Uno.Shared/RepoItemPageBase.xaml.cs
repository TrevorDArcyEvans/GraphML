namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

	public abstract partial class RepoItemPageBase : PageBase
	{
		protected NodeServer NodeServer;
		protected EdgeServer EdgeServer;

		public RepoItemPageBase()
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

		protected abstract void InitialiseUI(Repository repo);

		private async void Initialise(Repository repo)
		{
			EdgeServer = new EdgeServer(_config, _navArgs.Token, _innerHandler);
			NodeServer = new NodeServer(_config, _navArgs.Token, _innerHandler);

			InitialiseUI(repo);
		}

		protected void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}
	}
}
