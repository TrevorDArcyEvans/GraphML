namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class NodePage : PageBase
	{
		public NodePage()
		{
			InitializeComponent();
		}

		public ObservableCollection<Node> Nodes { get; set; } = new ObservableCollection<Node>();

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedRepository);
		}

		private async void Initialise(Repository repo)
		{
			var nodeServer = new NodeServer(_config, _navArgs.Token, _innerHandler);
			var nodes = await nodeServer.ByOwners(new[] { repo.Id });
			nodes.ToList()
		  .ForEach(node => MarshallToUI(() => Nodes.Add(node)));
		}


		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}
	}
}
