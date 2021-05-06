using System;
using System.Linq;
using System.Threading.Tasks;
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

    private bool _newDialogIsOpen;
    private string _newGraphName;
    private string _dlgNewGraphName;

    private bool _deleteDialogIsOpen;
    private Graph _deleteGraph;

    private void NewDialog()
    {
      _dlgNewGraphName = null;
      _newDialogIsOpen = true;
    }

    private async Task OkClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewGraphName))
      {
        return;
      }

      _newGraphName = _dlgNewGraphName;
      _newDialogIsOpen = false;
      var newGraph = await CreateNewGraph(_newGraphName);
      GotoBrowseGraphItems(newGraph);
    }

    private async Task<Graph> CreateNewGraph(string graphName)
    {
      var newGraph = new Graph(Guid.Parse(RepositoryId), Guid.Parse(OrganisationId), graphName);
      var newGraphs = await _graphServer.Create(new[] { newGraph });

      return newGraphs.Single();
    }

    private void ConfirmDeleteGraph(Graph chart)
    {
      _deleteGraph = chart;
      _deleteDialogIsOpen = true;
    }

    private async Task DeleteChart()
    {
      _deleteDialogIsOpen = false;
      await _graphServer.Delete(new[] { _deleteGraph });
      StateHasChanged();
    }

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
