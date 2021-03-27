namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public sealed class GraphNodePage : GraphItemPageBase
	{
		protected override async Task<IEnumerable<GraphItem>> GetGraphItems(Graph graph)
		{
			return await GraphNodeServer.ByOwners(new[] { graph.Id });
		}
	}
}
