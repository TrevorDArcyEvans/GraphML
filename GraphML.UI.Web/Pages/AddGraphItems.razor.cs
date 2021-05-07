using System;
using System.Threading.Tasks;
using BlazorTable;
using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class AddGraphItems
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

    private Node[] _data;
    private Table<Node> _table;

    protected override async Task OnInitializedAsync()
    {
      var dataPage = await _nodeServer.ByOwner(Guid.Parse(RepositoryId), 0, int.MaxValue, null);
      _data = dataPage.Items.ToArray();
    }

    private void AddSelectedGraphItems()
    {
      var selItems = _table.SelectedItems;
    }

    private void GotoBrowseGraphItems()
    {
      _navMgr.NavigateTo($"/BrowseGraphItems/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
