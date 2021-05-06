using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseItems
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

    #endregion

    private Node[] _nodes;
    private Edge[] _edges;

    private void GotoBrowseWhereUsed(Node item)
    {
      _navMgr.NavigateTo($"/BrowseWhereUsedNodes/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{item.Id}/{item.Name}");
    }

    private void GotoBrowseParents(Node item)
    {
      _navMgr.NavigateTo($"/BrowseParentNodes/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{item.Id}/{item.Name}");
    }

    private void GotoBrowseWhereUsed(Edge item)
    {
      _navMgr.NavigateTo($"/BrowseWhereUsedEdges/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{item.Id}/{item.Name}");
    }

    private void GotoBrowseParents(Edge item)
    {
      _navMgr.NavigateTo($"/BrowseParentEdges/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{item.Id}/{item.Name}");
    }

    private void GotoBrowseRepositories()
    {
      _navMgr.NavigateTo($"/BrowseRepositories/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}");
    }
  }
}
