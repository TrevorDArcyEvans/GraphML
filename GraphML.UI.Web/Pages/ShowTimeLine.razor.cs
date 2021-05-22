using BlazorContextMenu;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using QG = QuikGraph;

namespace GraphML.UI.Web.Pages
{
  public partial class ShowTimeLine
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

    [Parameter]
    public string TimelineName { get; set; }

    [Parameter]
    public string TimelineId { get; set; }

    [Parameter]
    public string EdgeItemAttributeDefinitionId { get; set; }

    [Parameter]
    public string EdgeItemAttributeDefinitionName { get; set; }

    #endregion

    #region Inject

    [Inject]
    private IBlazorContextMenuService _contextMenuService { get; set; }

    [Inject]
    private INodeServer _nodeServer { get; set; }

    [Inject]
    private IEdgeServer _edgeServer { get; set; }

    [Inject]
    private IGraphServer _graphServer { get; set; }

    [Inject]
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IGraphEdgeServer _graphEdgeServer { get; set; }

    [Inject]
    private IChartServer _chartServer { get; set; }

    [Inject]
    private IChartNodeServer _chartNodeServer { get; set; }

    [Inject]
    private IChartEdgeServer _chartEdgeServer { get; set; }

    // [Inject]
    // private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion
  }
}
