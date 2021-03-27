namespace GraphML.UI.Uno
{
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class GraphNodePage : PageBase
	{
		public GraphNodePage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedGraph);
		}

		private async void Initialise(Graph graph)
		{
			// TODO	Initialise
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(GraphPage), _navArgs);
		}
	}
}
