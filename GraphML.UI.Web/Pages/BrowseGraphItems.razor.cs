using Microsoft.AspNetCore.Components;

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

    private Node[] _nodes;
    private Edge[] _edges;

    private void GotoAddGraphItems()
    {
      _navMgr.NavigateTo($"/AddGraphItems/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }

    private void GotoBrowseGraphs()
    {
      _navMgr.NavigateTo($"/BrowseGraphs/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}");
    }
  }
}
