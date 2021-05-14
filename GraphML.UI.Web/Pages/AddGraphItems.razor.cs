using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTable;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

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

    [Inject]
    private INodeServer _nodeServer { get; set; }

    [Inject]
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    private List<Node> _data;
    private Table<Node> _table;

    private Guid _graphId;
    private Guid _orgid;

    private bool _addAllDialogIsOpen;
    private bool _isAddingItems;

    /// In Blazor WASM we would normally place
    /// data initialization here. However, on
    /// Blazor Server, a long running task in
    /// OnInitializedAsync will cause happen
    /// at the server and no spinner will display
    /// Instead, long running initialize methods
    /// should be moved to OnAfterRender with
    /// an added call to StateHasChange().
    //protected override async Task OnInitializedAsync()
    //{
    //    await LoadData();
    //}
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (firstRender)
      {
        _graphId = Guid.Parse(GraphId);
        _orgid = Guid.Parse(OrganisationId);

        // get GraphNodes already in Graph
        var allGraphNodesPage = await _graphNodeServer.ByOwner(_graphId, 0, int.MaxValue, null);
        var allGraphNodes = allGraphNodesPage.Items;
        var existRepoItemIds = allGraphNodes.Select(gn => gn.RepositoryItemId);

        // remove those GraphNodes from available Nodes
        var dataPage = await _nodeServer.ByOwner(Guid.Parse(RepositoryId), 0, int.MaxValue, null);
        _data = dataPage.Items
          .Where(n => !existRepoItemIds.Contains(n.Id))
          .ToList();

        StateHasChanged();
      }
    }

    private async Task AddSelectedGraphItems()
    {
      var selItems = _table.SelectedItems;
      var graphNodes = selItems.Select(n => new GraphNode(_graphId, _orgid, n.Id, n.Name));
      await _graphNodeServer.Create(graphNodes);

      // successfully created new GraphNodes, so remove underlying Nodes from available selection
      selItems.ForEach(item => _data.Remove(item));
    }

    private async Task AddAllRepositoryItems()
    {
      try
      {
        _addAllDialogIsOpen = false;
        _isAddingItems = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        _table.SelectedItems.Clear();
        _table.SelectedItems.AddRange(_data);
        await AddSelectedGraphItems();
      }
      finally
      {
        _isAddingItems = false;
      }
    }

    private void GotoBrowseGraphItems()
    {
      _navMgr.NavigateTo($"/BrowseGraphItems/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
