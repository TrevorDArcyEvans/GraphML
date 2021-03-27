namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class GraphEdgePage : PageBase
	{
		public GraphEdgePage()
		{
			InitializeComponent();
		}

		public ObservableCollection<GraphItem> GraphItems { get; set; } = new ObservableCollection<GraphItem>();

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedGraph);
		}

		private async void Initialise(Graph graph)
		{
			var graphItemServer = new GraphEdgeServer(_config, _navArgs.Token, _innerHandler);
			var graphItems = await graphItemServer.ByOwners(new[] { graph.Id });
			graphItems.ToList()
				.ForEach(graphItem => GraphItems.Add(graphItem));
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(GraphPage), _navArgs);
		}
	}
}
