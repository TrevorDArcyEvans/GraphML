namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class RepositoryManagerPage : PageBase
	{
		private RepositoryManagerServer _repoMgrServer;
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
			_repoMgrServer = new RepositoryManagerServer(_config, _navArgs.Token, _innerHandler);

			await LoadItems(selOrg);
		}

		private async Task LoadItems(Organisation selOrg)
		{
			RepositoryManagers.Clear();

			var repoMgrs = await _repoMgrServer.ByOwners(new[] { selOrg.Id }, _pageIndex, PageSize);
			repoMgrs.ToList()
		  .ForEach(repoMgr => RepositoryManagers.Add(repoMgr));
		}

    private async void Previous_Click(object sender, object args)
		{
			if (_pageIndex > 1)
			{
				_pageIndex--;
			}

			OnPropertyChanged(nameof(_pageIndex));
			await LoadItems(_navArgs.SelectedOrganisation);
		}

    private async void Next_Click(object sender, object args)
		{
			_pageIndex++;

			OnPropertyChanged(nameof(_pageIndex));
			await LoadItems(_navArgs.SelectedOrganisation);
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
