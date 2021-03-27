namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Linq;

	public sealed partial class GraphEdgePage : GraphItemPageBase
	{
		protected override async void InitialiseUI(Graph graph)
		{
			var repoItems = await GraphEdgeServer.ByOwners(new[] { graph.Id });
			repoItems.ToList()
		  .ForEach(repoItem => MarshallToUI(() => GraphItems.Add(repoItem)));
		}
	}
}
