using System;
using System.Linq;
using System.Threading.Tasks;
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

    private bool _newDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private bool _deleteDialogIsOpen;
    private Repository _deleteItem;

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
      var newItem = await CreateNewItem(_newItemName);
      GotoBrowseGraphs(newItem);
    }

    private async Task<Repository> CreateNewItem(string itemName)
    {
      var newItem = new Repository(Guid.Parse(RepositoryManagerId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _repoServer.Create(new[] { newItem });

      return newItems.Single();
    }

    private void ConfirmDelete(Repository item)
    {
      _deleteItem = item;
      _deleteDialogIsOpen = true;
    }

    private async Task Delete()
    {
      _deleteDialogIsOpen = false;
      await _repoServer.Delete(new[] { _deleteItem });
      StateHasChanged();
    }

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
