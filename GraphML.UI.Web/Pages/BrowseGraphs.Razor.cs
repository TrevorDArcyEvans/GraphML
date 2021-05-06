using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseGraphs
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

    private Graph[] _graphs;

    private void GotoBrowseGraphItems(Graph graph)
    {
      _navMgr.NavigateTo($"/BrowseGraphItems/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{graph.Id}/{graph.Name}");
    }

    private void GotoBrowseCharts(Graph graph)
    {
      _navMgr.NavigateTo($"/BrowseCharts/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{graph.Id}/{graph.Name}");
    }

    private void GotoBrowseAnalyses(Graph graph)
    {
      _navMgr.NavigateTo($"/BrowseAnalyses/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{graph.Id}/{graph.Name}");
    }

    private void GotoBrowseResults(Graph graph)
    {
      _navMgr.NavigateTo($"/BrowseResults/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{graph.Id}/{graph.Name}");
    }

    private void GotoBrowseRepositories()
    {
      _navMgr.NavigateTo($"/BrowseRepositories/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}");
    }
  }
}
