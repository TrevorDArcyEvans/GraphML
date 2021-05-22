using BlazorContextMenu;
using Microsoft.AspNetCore.Components;

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

    #endregion
  }
}
