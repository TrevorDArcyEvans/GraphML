using System;
using System.Collections.Concurrent;
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
  public partial class AddGraphEdges
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
    private IEdgeServer _edgeServer { get; set; }

    [Inject]
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IGraphEdgeServer _graphEdgeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private const int ChunkSize = 1000;
    private const int DegreeofParallelism = 10;

    private List<Edge> _data;
    private Table<Edge> _table;

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
        _orgId = Guid.Parse(OrganisationId);
        _repoId = Guid.Parse(RepositoryId);
        _graphId = Guid.Parse(GraphId);
        
        var lockObj = new object();

        // get GraphEdges already in Graph
        var allGraphEdgesCount = await _graphEdgeServer.Count(_graphId);
        var existRepoItemIds = new List<Guid>(allGraphEdgesCount);
        var numGraphEdgeChunks = (allGraphEdgesCount / ChunkSize) + 1;
        var chunkRange = Enumerable.Range(0, numGraphEdgeChunks);
        await chunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
        {
          var allGraphEdgesPage = await _graphEdgeServer.ByOwner(_graphId, i + 1, ChunkSize, null);
          var allGraphEdges = allGraphEdgesPage.Items;
          var allGraphEdgesRepoItemIds = allGraphEdges.Select(gn => gn.RepositoryItemId);
          lock (lockObj)
          {
            existRepoItemIds.AddRange(allGraphEdgesRepoItemIds);
          }
        });

        // remove those GraphEdges from available Edges
        var dataCount = await _edgeServer.Count(_repoId);
        _data = new List<Edge>(dataCount);
        var numDataChunks = (dataCount / ChunkSize) + 1;
        var dataRange = Enumerable.Range(0, numDataChunks);
        await dataRange.ParallelForEachAsync(DegreeofParallelism, async i =>
        {
          var dataPage = await _edgeServer.ByOwner(_repoId, i + 1, ChunkSize, null);
          var dataPageEdges = dataPage.Items
            .Where(n => !existRepoItemIds.Contains(n.Id))
            .ToList();
          lock (lockObj)
          {
            _data.AddRange(dataPageEdges);
          }
        });

        StateHasChanged();
      }
    }

    private async Task AddGraphItems(List<Edge> items)
    {
      try
      {
        _addAllDialogIsOpen = false;
        _isAddingItems = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        var nodeIds = items
          .SelectMany(e => new[] { e.SourceId, e.TargetId }).Distinct()
          .ToList();
        var graphNodes = await GetGraphNodesByOwners(nodeIds);
        var graphNodeRepoIds = graphNodes.Select(gn => gn.RepositoryItemId);
        var missingGraphNodeRepoIds = nodeIds.Except(graphNodeRepoIds);
        var missingGraphNodeRepo = await _nodeServer.ByIds(missingGraphNodeRepoIds);
        var missingGraphNodes = missingGraphNodeRepo
          .Select(n => new GraphNode(_graphId, _orgId, n.Id, n.Name))
          .ToList();
        _ = await _graphNodeServer.Create(missingGraphNodes);
        graphNodes.AddRange(missingGraphNodes);

        var graphEdges = items
          .Select(e =>
          {
            var source = graphNodes.SingleOrDefault(gn => gn.RepositoryItemId == e.SourceId);
            var target = graphNodes.SingleOrDefault(gn => gn.RepositoryItemId == e.TargetId);
            return new GraphEdge(
              _graphId,
              _orgId,
              e.Id,
              e.Name,
              source.Id,
              target.Id);
          });
        await _graphEdgeServer.Create(graphEdges);

        // successfully created new GraphEdges, so remove underlying Edges from available selection
        items.ForEach(item => _data.Remove(item));
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

    private async Task<List<GraphNode>> GetGraphNodesByOwners(List<Guid> nodeIds)
    {
      var numData = nodeIds.Count;
      var numChunks = (numData / ChunkSize) + 1;
      var chunkRange = Enumerable.Range(0, numChunks);
      var retval = new ConcurrentBag<GraphNode>();
      await chunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
      {
        var graphNodesPage = await _graphNodeServer.ByOwners(nodeIds, i + 1, ChunkSize, null);
        var graphNodes = graphNodesPage.Items.ToList();
        graphNodes.ForEach(gn => retval.Add(gn));
      });

      return retval.ToList();
    }
  }
}
