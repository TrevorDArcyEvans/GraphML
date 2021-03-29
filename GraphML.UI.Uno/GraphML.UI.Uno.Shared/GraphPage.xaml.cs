namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class GraphPage : PageBase
	{
		private Graph _selectedGraph;
		private GraphServer _graphServer;

		public GraphPage() :
			base()
		{
			InitializeComponent();
		}

		public ObservableCollection<Graph> Graphs { get; set; } = new ObservableCollection<Graph>();

		public Graph SelectedGraph
		{
			get => _selectedGraph;
			set => SetProperty(ref _selectedGraph, value);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedRepository);
		}

		private async void Initialise(Repository repo)
		{
			_graphServer = new GraphServer(_config, _navArgs.Token, _innerHandler);

			await LoadItems(repo);
		}

		private async Task LoadItems(Repository repo)
		{
			Graphs.Clear();

			var graphs = await _graphServer.ByOwners(new[] { repo.Id }, _pageIndex, PageSize);
			graphs.ToList()
			.ForEach(graph => Graphs.Add(graph));
		}

		private void GraphNodes_Click(object sender, object args)
		{
			_navArgs.SelectedGraph = SelectedGraph;
			Frame.Navigate(typeof(GraphNodePage), _navArgs);
		}

		private void GraphEdges_Click(object sender, object args)
		{
			_navArgs.SelectedGraph = SelectedGraph;
			Frame.Navigate(typeof(GraphEdgePage), _navArgs);
		}

		private async void Previous_Click(object sender, object args)
		{
			if (_pageIndex > 1)
			{
				_pageIndex--;
			}

			OnPropertyChanged(nameof(_pageIndex));
			await LoadItems(_navArgs.SelectedRepository);
		}

		private async void Next_Click(object sender, object args)
		{
			_pageIndex++;

			OnPropertyChanged(nameof(_pageIndex));
			await LoadItems(_navArgs.SelectedRepository);
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}
	}
}
