namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class RepositoryPage : PageBase
	{
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
			var repoTask = Task.Factory.StartNew(async () =>
			{
				var repoServer = new RepositoryServer(_config, _navArgs.Token, _innerHandler);
				var repos = await repoServer.ByOwners(new[] { repoMgr.Id }, 1, 20); //TODO paging
				repos.ToList()
			  .ForEach(repo => MarshallToUI(() => Repositories.Add(repo)));
			});

			var repoAttrTask = Task.Factory.StartNew(async () =>
			{
				var repoAttrServer = new RepositoryItemAttributeDefinitionServer(_config, _navArgs.Token, _innerHandler);
				var repoAttrs = await repoAttrServer.ByOwners(new[] { repoMgr.Id }, 1, 20); //TODO paging
				repoAttrs.ToList()
			  .ForEach(attr => MarshallToUI(() => RepositoryItemAttributes.Add(attr)));
			});

			var nodeAttrTask = Task.Factory.StartNew(async () =>
			{
				var nodeAttrServer = new NodeItemAttributeDefinitionServer(_config, _navArgs.Token, _innerHandler);
				var nodeAttrs = await nodeAttrServer.ByOwners(new[] { repoMgr.Id }, 1, 20); //TODO paging
				nodeAttrs.ToList()
			  .ForEach(attr => MarshallToUI(() => NodeItemAttributes.Add(attr)));
			});

			var edgeAttrTask = Task.Factory.StartNew(async () =>
			{
				var edgeAttrServer = new EdgeItemAttributeDefinitionServer(_config, _navArgs.Token, _innerHandler);
				var edgeAttrs = await edgeAttrServer.ByOwners(new[] { repoMgr.Id }, 1, 20); //TODO paging
				edgeAttrs.ToList()
			  .ForEach(attr => MarshallToUI(() => EdgeItemAttributes.Add(attr)));
			});

			await Task.WhenAll(repoTask, repoAttrTask, nodeAttrTask, edgeAttrTask);
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
