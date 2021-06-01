using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTable;
using GraphML.Interfaces.Server;
using GraphML.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Web.Pages
{
  public partial class AddGraphNodes
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

    #region Inject

    [Inject]
    private INodeServer _nodeServer { get; set; }

    [Inject]
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private const int ChunkSize = 1000;
    private const int DegreeofParallelism = 10;

    private List<Node> _data;
    private Table<Node> _table;

    private Guid _orgId;
    private Guid _repoId;
    private Guid _graphId;

    private bool _addAllDialogIsOpen;
    private bool _isAddingItems;

    // In Blazor WASM we would normally place
    // data initialization here. However, on
    // Blazor Server, a long running task in
    // OnInitializedAsync will cause happen
    // at the server and no spinner will display
    // Instead, long running initialize methods
    // should be moved to OnAfterRender with
    // an added call to StateHasChange().
    //protected override async Task OnInitializedAsync()
    //{
    //    await LoadData();
    //}
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (firstRender)
      {
        _repoId = Guid.Parse(RepositoryId);
        _graphId = Guid.Parse(GraphId);
        _orgId = Guid.Parse(OrganisationId);

        var lockObj = new object();

        // get GraphNodes already in Graph
        var allGraphNodesCount = await _graphNodeServer.Count(_graphId);
        var existRepoItemIds = new List<Guid>(allGraphNodesCount);
        var numGraphNodeChunks = (allGraphNodesCount / ChunkSize) + 1;
        var chunkRange = Enumerable.Range(0, numGraphNodeChunks);
        await chunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
        {
          var allGraphNodesPage = await _graphNodeServer.ByOwner(_graphId, i + 1, ChunkSize, null);
          var allGraphNodes = allGraphNodesPage.Items;
          var allGraphNodeRepoItemIds = allGraphNodes.Select(gn => gn.RepositoryItemId);
          lock (lockObj)
          {
            existRepoItemIds.AddRange(allGraphNodeRepoItemIds);
          }
        });

        // remove those GraphNodes from available Nodes
        var dataCount = await _nodeServer.Count(_repoId);
        _data = new List<Node>(dataCount);
        var numDataChunks = (dataCount / ChunkSize) + 1;
        var dataRange = Enumerable.Range(0, numDataChunks);
        await dataRange.ParallelForEachAsync(DegreeofParallelism, async i =>
        {
          var dataPage = await _nodeServer.ByOwner(Guid.Parse(RepositoryId), i + 1, ChunkSize, null);
          var dataPageNodes = dataPage.Items
            .Where(n => !existRepoItemIds.Contains(n.Id))
            .ToList();
          lock (lockObj)
          {
            _data.AddRange(dataPageNodes);
          }
        });

        StateHasChanged();
      }
    }

    private async Task AddGraphItems(List<Node> items)
    {
      try
      {
        _addAllDialogIsOpen = false;
        _isAddingItems = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        var graphNodes = items.Select(n => new GraphNode(_graphId, _orgId, n.Id, n.Name)).ToList();
        var numGraphNodeChunks = (graphNodes.Count / ChunkSize) + 1;
        var chunkRange = Enumerable.Range(0, numGraphNodeChunks);
        await chunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
        {
          var dataChunk = graphNodes.Skip(i * ChunkSize).Take(ChunkSize);
          await _graphNodeServer.Create(dataChunk);
        });

        // successfully created new GraphNodes, so remove underlying Nodes from available selection
        // NOTE:  'items' and '_data' point back to the same underlying info,
        // so we have to create a copy ('shadowItems') of 'items' otherwise we are trying to 
        // modify a list whilst iterating over it.
        var shadowItems = items.Select(item => item).ToList();
        shadowItems.ForEach(item => _data.Remove(item));
      }
      finally
      {
        _isAddingItems = false;
      }
    }

    private async Task AddSelectedItems()
    {
      await AddGraphItems(_table.SelectedItems);
    }

    private async Task AddFilteredItems()
    {
      await AddGraphItems(_table.FilteredItems.ToList());
    }

    private async Task AddAllItems()
    {
      await AddGraphItems(_data);
    }
  }
}
