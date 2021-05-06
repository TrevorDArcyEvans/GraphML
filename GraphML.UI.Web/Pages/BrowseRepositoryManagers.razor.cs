using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseRepositoryManagers
  {
    [Parameter]
    public string OrganisationName { get; set; }

    [Parameter]
    public string OrganisationId { get; set; }

    private RepositoryManager[] _repoMgrs;

    private void GotoBrowseRepositories(RepositoryManager repoMgr)
    {
      _navMgr.NavigateTo($"/BrowseRepositories/{OrganisationId}/{OrganisationName}/{repoMgr.Id}/{repoMgr.Name}");
    }

    private void GotoImporter()
    {
      _navMgr.NavigateTo($"/Importer/{OrganisationId}/{OrganisationName}");
    }

    private void GotoBrowseOrganisations()
    {
      _navMgr.NavigateTo($"/BrowseOrganisations");
    }
  }
}
