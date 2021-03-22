﻿using System.Collections.ObjectModel;
using GraphML.UI.Uno.Server;

namespace GraphML.UI.Uno
{
	using System.Linq;
	using Windows.UI.Xaml.Navigation;

	public sealed partial class GraphPage : PageBase
	{
		public GraphPage() :
			base()
		{
			InitializeComponent();
		}

		public ObservableCollection<Graph> Graphs { get; set; } = new ObservableCollection<Graph>();
		public Graph SelectedGraph { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			_navArgs = (BreadcrumbTrail)e.Parameter;

			base.OnNavigatedTo(e);

			Initialise(_navArgs.SelectedRepository);
		}

		private async void Initialise(Repository repo)
		{
			var graphServer = new GraphServer(_config, _navArgs.Token, _innerHandler);
			var graphs = await graphServer.ByOwners(new[] { repo.Id });
			graphs.ToList()
		  .ForEach(graph => Graphs.Add(graph));
		}

		private void Graph_Click(object sender, object args)
		{
			_navArgs.SelectedGraph = SelectedGraph;
			// TODO	  Frame.Navigate(typeof(GraphItemPage), _navArgs); aka NodesEdgesPage
		}

		private void Back_Click(object sender, object args)
		{
			Frame.Navigate(typeof(RepositoryPage), _navArgs);
		}
	}
}