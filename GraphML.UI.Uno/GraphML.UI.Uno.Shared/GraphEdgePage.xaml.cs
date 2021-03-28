namespace GraphML.UI.Uno
{
	using GraphML.UI.Uno.Server;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public sealed class GraphEdgePage : GraphItemPageBase
	{
		protected override async Task<IEnumerable<GraphItem>> GetGraphItems(Graph graph, int pageIndex, int pageSize)
		{
			return await GraphEdgeServer.ByOwners(new[] { graph.Id }, _pageIndex, PageSize);
		}
	}
}
