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
    private string _newItemName;
    private string _dlgNewItemName;

    private bool _deleteDialogIsOpen;
    private Graph _deleteItem;

    private void NewDialog()
    {
      _dlgNewItemName = null;
      _newDialogIsOpen = true;
    }

    private async Task OkClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewItemName))
      {
        return;
      }

      _newItemName = _dlgNewItemName;
      _newDialogIsOpen = false;
      var newGraph = await CreateNewItem(_newItemName);
      GotoBrowseGraphItems(newGraph);
    }

    private async Task<Graph> CreateNewItem(string itemName)
    {
      var newItem = new Graph(Guid.Parse(RepositoryId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _graphServer.Create(new[] { newItem });

      return newItems.Single();
    }

    private void ConfirmDelete(Graph item)
    {
      _deleteItem = item;
      _deleteDialogIsOpen = true;
    }

    private async Task Delete()
    {
      _deleteDialogIsOpen = false;
      await _graphServer.Delete(new[] { _deleteItem });
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
  }
}
