namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class GraphPage : PageBase
	{
		private Graph _selectedGraph;

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
			var graphServer = new GraphServer(_config, _navArgs.Token, _innerHandler);
			var graphs = await graphServer.ByOwners(new[] { repo.Id }, _pageIndex, PageSize);
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

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}
	}
}
