namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class RepositoryPage : PageBase
	{
		private int _pageIndexRepo = 1;
		private int _pageIndexRepoItemAttr = 1;
		private int _pageIndexNodeItemAttr = 1;
		private int _pageIndexEdgeItemAttr = 1;

		private RepositoryServer repoServer;
		private RepositoryItemAttributeDefinitionServer repoAttrDefServer;
		private NodeItemAttributeDefinitionServer nodeAttrDefServer;
		private EdgeItemAttributeDefinitionServer edgeAttrDefServer;

		private Repository _selectedRepository;

		public RepositoryPage()
		{
			InitializeComponent();
		}

		public ObservableCollection<Repository> Repositories { get; set; } = new ObservableCollection<Repository>();

		public Repository SelectedRepository
		{
			get => _selectedRepository;
			set => SetProperty(ref _selectedRepository, value);
		}

		public ObservableCollection<RepositoryItemAttributeDefinition> RepositoryItemAttributes { get; set; } = new ObservableCollection<RepositoryItemAttributeDefinition>();
		public ObservableCollection<NodeItemAttributeDefinition> NodeItemAttributes { get; set; } = new ObservableCollection<NodeItemAttributeDefinition>();
		public ObservableCollection<EdgeItemAttributeDefinition> EdgeItemAttributes { get; set; } = new ObservableCollection<EdgeItemAttributeDefinition>();

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedRepositoryManager);
		}

		private async void Initialise(RepositoryManager repoMgr)
		{
			repoServer = new RepositoryServer(_config, _navArgs.Token, _innerHandler);
			repoAttrDefServer = new RepositoryItemAttributeDefinitionServer(_config, _navArgs.Token, _innerHandler);
			nodeAttrDefServer = new NodeItemAttributeDefinitionServer(_config, _navArgs.Token, _innerHandler);
			edgeAttrDefServer = new EdgeItemAttributeDefinitionServer(_config, _navArgs.Token, _innerHandler);

			var repoTask = LoadRepositories(repoMgr);
			var repoAttrDefTask = LoadRepositoryAttributeDefinitions(repoMgr);
			var nodeAttrDefTask = LoadNodeAttributeDefinitions(repoMgr);
			var edgeAttrDefTask = LoadEdgeAttributeDefinitions(repoMgr);

			await Task.WhenAll(repoTask, repoAttrDefTask, nodeAttrDefTask, edgeAttrDefTask);
		}

		private Task LoadRepositories(RepositoryManager repoMgr)
		{
			return Task.Factory.StartNew(async () =>
	  {
		  MarshallToUI(() => Repositories.Clear());
		  var repos = await repoServer.ByOwners(new[] { repoMgr.Id }, _pageIndexRepo, PageSize);
		  repos.ToList()
			  .ForEach(repo => MarshallToUI(() => Repositories.Add(repo)));
	  });
		}

		private Task LoadRepositoryAttributeDefinitions(RepositoryManager repoMgr)
		{
			return Task.Factory.StartNew(async () =>
			{
				MarshallToUI(() => RepositoryItemAttributes.Clear());
				var repoAttrs = await repoAttrDefServer.ByOwners(new[] { repoMgr.Id }, _pageIndexRepoItemAttr, PageSize);
				repoAttrs.ToList()
					.ForEach(attr => MarshallToUI(() => RepositoryItemAttributes.Add(attr)));
			});
		}

		private Task LoadNodeAttributeDefinitions(RepositoryManager repoMgr)
		{
			return Task.Factory.StartNew(async () =>
			{
				MarshallToUI(() => NodeItemAttributes.Clear());
				var nodeAttrs = await nodeAttrDefServer.ByOwners(new[] { repoMgr.Id }, _pageIndexNodeItemAttr, PageSize);
				nodeAttrs.ToList()
					.ForEach(attr => MarshallToUI(() => NodeItemAttributes.Add(attr)));
			});
		}

		private Task LoadEdgeAttributeDefinitions(RepositoryManager repoMgr)
		{
			return Task.Factory.StartNew(async () =>
			{
				MarshallToUI(() => EdgeItemAttributes.Clear());
				var edgeAttrs = await edgeAttrDefServer.ByOwners(new[] { repoMgr.Id }, _pageIndexEdgeItemAttr, PageSize);
				edgeAttrs.ToList()
					.ForEach(attr => MarshallToUI(() => EdgeItemAttributes.Add(attr)));
			});
		}

		private async void PreviousRepository_Click(object sender, object args)
		{
			if (_pageIndexRepo > 1)
			{
				_pageIndexRepo--;
			}

			OnPropertyChanged(nameof(_pageIndexRepo));
			await LoadRepositories(_navArgs.SelectedRepositoryManager);
		}

		private async void NextRepository_Click(object sender, object args)
		{
			_pageIndexRepo++;

			OnPropertyChanged(nameof(_pageIndexRepo));
			await LoadRepositories(_navArgs.SelectedRepositoryManager);
		}

		private async void PreviousRepoItemAttr_Click(object sender, object args)
		{
			if (_pageIndexRepoItemAttr > 1)
			{
				_pageIndexRepoItemAttr--;
			}

			OnPropertyChanged(nameof(_pageIndexRepoItemAttr));
			await LoadRepositoryAttributeDefinitions(_navArgs.SelectedRepositoryManager);
		}

		private async void NextRepoItemAttr_Click(object sender, object args)
		{
			_pageIndexRepoItemAttr++;

			OnPropertyChanged(nameof(_pageIndexRepoItemAttr));
			await LoadRepositoryAttributeDefinitions(_navArgs.SelectedRepositoryManager);
		}

		private async void PreviousNodeItemAttr_Click(object sender, object args)
		{
			if (_pageIndexNodeItemAttr > 1)
			{
				_pageIndexNodeItemAttr--;
			}

			OnPropertyChanged(nameof(_pageIndexNodeItemAttr));
			await LoadNodeAttributeDefinitions(_navArgs.SelectedRepositoryManager);
		}

		private async void NextNodeItemAttr_Click(object sender, object args)
		{
			_pageIndexNodeItemAttr++;

			OnPropertyChanged(nameof(_pageIndexNodeItemAttr));
			await LoadNodeAttributeDefinitions(_navArgs.SelectedRepositoryManager);
		}

		private async void PreviousEdgeItemAttr_Click(object sender, object args)
		{
			if (_pageIndexEdgeItemAttr > 1)
			{
				_pageIndexEdgeItemAttr--;
			}

			OnPropertyChanged(nameof(_pageIndexEdgeItemAttr));
			await LoadEdgeAttributeDefinitions(_navArgs.SelectedRepositoryManager);
		}

		private async void NextEdgeItemAttr_Click(object sender, object args)
		{
			_pageIndexEdgeItemAttr++;

			OnPropertyChanged(nameof(_pageIndexEdgeItemAttr));
			await LoadEdgeAttributeDefinitions(_navArgs.SelectedRepositoryManager);
		}

		private void Graphs_Click(object sender, object args)
		{
			_navArgs.SelectedRepository = SelectedRepository;
			Frame.Navigate(typeof(GraphPage), _navArgs);
		}

		private void Nodes_Click(object sender, object args)
		{
			_navArgs.SelectedRepository = SelectedRepository;
			Frame.Navigate(typeof(NodePage), _navArgs);
		}

		private void Edges_Click(object sender, object args)
		{
			_navArgs.SelectedRepository = SelectedRepository;
			Frame.Navigate(typeof(EdgePage), _navArgs);
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryManagerPage), _navArgs);
		}
	}
}
