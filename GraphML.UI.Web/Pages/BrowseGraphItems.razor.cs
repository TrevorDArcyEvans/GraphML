using System.Threading.Tasks;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseGraphItems
  {
    #region Parameters

    [Parameter]
    public string OrganisationName { get; set; }

    [Parameter]
    public string OrganisationId { get; set; }

    [Parameter]
    public string RepositoryManagerName { get; set; }

    [Parameter]
    public string RepositoryManagerId { get; set; }

    [Parameter]
    public string RepositoryName { get; set; }

    [Parameter]
    public string RepositoryId { get; set; }

    [Parameter]
    public string GraphName { get; set; }

    [Parameter]
    public string GraphId { get; set; }

    #endregion

    #region Inject

    [Inject]
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IGraphEdgeServer _graphEdgeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private GraphNode[] _nodes;
    private GraphEdge[] _edges;

    private bool _isBusy;

    private async Task DeleteGraphNode(GraphNode graphItem)
    {
      try
      {
        _isBusy = true;
        await _graphNodeServer.Delete(new[] { graphItem });
        StateHasChanged();
      }
      finally
      {
        _isBusy = false;
      }
    }

    private async Task DeleteGraphEdge(GraphEdge graphItem)
    {
      try
      {
        _isBusy = true;
        await _graphEdgeServer.Delete(new[] { graphItem });
        StateHasChanged();
      }
      finally
      {
        _isBusy = false;
      }
    }

    private void GotoAddGraphNodes()
    {
      _navMgr.NavigateTo($"/AddGraphNodes/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }

    private void GotoAddGraphEdges()
    {
      _navMgr.NavigateTo($"/AddGraphEdges/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
