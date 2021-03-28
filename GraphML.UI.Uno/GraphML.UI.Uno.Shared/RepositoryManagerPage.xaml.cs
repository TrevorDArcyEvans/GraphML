namespace GraphML.UI.Uno
{
	using System.Collections.ObjectModel;
	using System.Linq;
	using Windows.UI.Xaml.Navigation;
	using GraphML.UI.Uno.Server;

	public sealed partial class RepositoryManagerPage : PageBase
	{
		private RepositoryManager _selectedRepositoryManager;

		public RepositoryManagerPage() :
			  base()
		{
			InitializeComponent();
		}

		public ObservableCollection<RepositoryManager> RepositoryManagers { get; set; } = new ObservableCollection<RepositoryManager>();

		public RepositoryManager SelectedRepositoryManager
		{
			get => _selectedRepositoryManager;
			set => SetProperty(ref _selectedRepositoryManager, value);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedOrganisation);
		}

		private async void Initialise(Organisation selOrg)
		{
			var repoMgrServer = new RepositoryManagerServer(_config, _navArgs.Token, _innerHandler);
			var repoMgrs = await repoMgrServer.ByOwners(new[] { selOrg.Id }, 1, 20); //TODO paging
			repoMgrs.ToList()
		  .ForEach(repoMgr => RepositoryManagers.Add(repoMgr));
		}

		private void Repository_Click(object sender, object args)
		{
			_navArgs.SelectedRepositoryManager = SelectedRepositoryManager;
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(OrganisationPage), _navArgs);
		}
	}
}
