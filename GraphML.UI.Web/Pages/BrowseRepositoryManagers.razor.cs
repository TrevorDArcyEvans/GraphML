using System;
using System.Linq;
using System.Threading.Tasks;
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

    private bool _isBusy;

    private bool _newDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private bool _deleteDialogIsOpen;
    private RepositoryManager _deleteItem;

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
      GotoBrowseRepositories(newItem);
    }

    private async Task<RepositoryManager> CreateNewItem(string itemName)
    {
      var newItem = new RepositoryManager(Guid.Parse(OrganisationId), itemName);
      var newItems = await _repoMgrServer.Create(new[] { newItem });

      return newItems.Single();
    }

    private void GotoBrowseAttributes(RepositoryManager repoMgr)
    {
      _navMgr.NavigateTo($"/BrowseAttributes/{OrganisationId}/{OrganisationName}/{repoMgr.Id}/{repoMgr.Name}/");
    }

    private void ConfirmDelete(RepositoryManager item)
    {
      _deleteItem = item;
      _deleteDialogIsOpen = true;
    }

    private async Task Delete()
    {
      _deleteDialogIsOpen = false;
      try
      {
        _isBusy = true;
        await _repoMgrServer.Delete(new[] { _deleteItem });
        StateHasChanged();
      }
      finally
      {
        _isBusy = false;
      }
    }

    private void GotoBrowseRepositories(RepositoryManager repoMgr)
    {
      _navMgr.NavigateTo($"/BrowseRepositories/{OrganisationId}/{OrganisationName}/{repoMgr.Id}/{repoMgr.Name}");
    }

    private void GotoImporter()
    {
      _navMgr.NavigateTo($"/Importer/{OrganisationId}/{OrganisationName}");
    }
  }
}
