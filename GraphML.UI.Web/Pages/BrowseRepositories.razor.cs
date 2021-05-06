using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseRepositories
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

    #endregion

    private Repository[] _repos;

    private void GotoBrowseGraphs(Repository repo)
    {
      _navMgr.NavigateTo($"/BrowseGraphs/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{repo.Id}/{repo.Name}");
    }

    private void GotoBrowseItems(Repository repo)
    {
      _navMgr.NavigateTo($"/BrowseItems/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{repo.Id}/{repo.Name}");
    }

    private void GotoBrowseRepositoryManagers()
    {
      _navMgr.NavigateTo($"/BrowseRepositoryManagers/{OrganisationId}/{OrganisationName}");
    }
  }
}
