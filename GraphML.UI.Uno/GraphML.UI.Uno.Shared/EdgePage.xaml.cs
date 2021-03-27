namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class EdgePage : PageBase
	{
		public EdgePage()
		{
			InitializeComponent();
		}

		public ObservableCollection<Edge> Edges { get; set; } = new ObservableCollection<Edge>();

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedRepository);
		}

		private async void Initialise(Repository repo)
		{
			var edgeServer = new EdgeServer(_config, _navArgs.Token, _innerHandler);
			var edges = await edgeServer.ByOwners(new[] { repo.Id });
			edges.ToList()
		  .ForEach(edge => MarshallToUI(() => Edges.Add(edge)));
		}

    private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}
	}
}
