using GraphML.UI.Uno.Server;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GraphML.UI.Uno
{
    public abstract partial class GraphItemPageBase : PageBase
	{
		protected GraphNodeServer GraphNodeServer;
		protected GraphEdgeServer GraphEdgeServer;

		public GraphItemPageBase()
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

		protected abstract void InitialiseUI(Graph graph);

		private async void Initialise(Graph graph)
		{
			GraphNodeServer = new GraphNodeServer(_config, _navArgs.Token, _innerHandler);
			GraphEdgeServer = new GraphEdgeServer(_config, _navArgs.Token, _innerHandler);

			InitialiseUI(graph);
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(GraphPage), _navArgs);
		}
	}
}
