namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

	public abstract partial class GraphItemPageBase : PageBase
	{
		protected GraphNodeServer GraphNodeServer;
		protected GraphEdgeServer GraphEdgeServer;

		public GraphItemPageBase()
		{
			InitializeComponent();
		}

		public ObservableCollection<GraphItem> GraphItems { get; set; } = new ObservableCollection<GraphItem>();

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			await Initialise(_navArgs.SelectedGraph);
		}

		protected abstract Task<IEnumerable<GraphItem>> GetGraphItems(Graph graph);

		private async Task Initialise(Graph graph)
		{
			GraphNodeServer = new GraphNodeServer(_config, _navArgs.Token, _innerHandler);
			GraphEdgeServer = new GraphEdgeServer(_config, _navArgs.Token, _innerHandler);

			var graphItems = await GetGraphItems(graph);
			graphItems.ToList()
				.ForEach(graphItem => MarshallToUI(() => GraphItems.Add(graphItem)));
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(GraphPage), _navArgs);
		}
	}
}
