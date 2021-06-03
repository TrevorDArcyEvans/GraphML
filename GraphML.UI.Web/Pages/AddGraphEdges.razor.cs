using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTable;
using GraphML.Interfaces.Server;
using GraphML.UI.Web.Widgets;
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

    private const int ChunkSize = 10000;
    private const int DegreeofParallelism = 10;

    private Edge[] _data;
    private MatTableEx<Edge> _table;

    private Guid _orgId;
    private Guid _repoId;
    private Guid _graphId;

    private bool _addAllDialogIsOpen;
    private bool _isAddingItems;

    protected override void OnInitialized()
    {
      _orgId = Guid.Parse(OrganisationId);
      _repoId = Guid.Parse(RepositoryId);
      _graphId = Guid.Parse(GraphId);
    }

    private async Task AddGraphItems(string searchTerm = null)
    {
      try
      {
        _addAllDialogIsOpen = false;
        _isAddingItems = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        // create missing GraphNodes
        var repoItemsPage = await _edgeServer.ByOwner(_repoId, 1, 1, searchTerm);
        var numRepoItems = (int) repoItemsPage.TotalCount;
        var numChunks = numRepoItems / ChunkSize + 1;
        var edgeChunkRange = Enumerable.Range(0, numChunks);
        var nodeIds = new ConcurrentBag<Guid>();
        var items = new ConcurrentBag<Edge>();
        await edgeChunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
        {
          var dataChunkPage = await _edgeServer.ByOwner(_repoId, i + 1, ChunkSize, searchTerm);
          var dataChunk = dataChunkPage.Items
            .ToList();
          dataChunk.SelectMany(e => new[] { e.SourceId, e.TargetId })
            .Distinct()
            .ToList()
            .ForEach(id => nodeIds.Add(id));
          dataChunk.ForEach(e => items.Add(e));
        });
        var graphNodes = await GetGraphNodesByOwners(nodeIds.ToList());
        var graphNodeRepoIds = graphNodes.Select(gn => gn.RepositoryItemId);
        var missingGraphNodeRepoIds = nodeIds.Except(graphNodeRepoIds);

        // TODO   chunk
        var missingGraphNodeRepo = await _nodeServer.ByIds(missingGraphNodeRepoIds);
        var missingGraphNodes = missingGraphNodeRepo
          .Select(n => new GraphNode(_graphId, _orgId, n.Id, n.Name))
          .ToList();
        var numMissGraphNodeChunks = (missingGraphNodes.Count / ChunkSize) + 1;
        var missChunkRange = Enumerable.Range(0, numMissGraphNodeChunks);
        await missChunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
        {
          var dataChunk = missingGraphNodes.Skip(i * ChunkSize).Take(ChunkSize);
          _ = await _graphNodeServer.Create(dataChunk);
        });
        graphNodes.AddRange(missingGraphNodes);

        // create new GraphEdges
        var graphEdges = items
          .Select(e =>
          {
            var source = graphNodes.Single(gn => gn.RepositoryItemId == e.SourceId);
            var target = graphNodes.Single(gn => gn.RepositoryItemId == e.TargetId);
            return new GraphEdge(
              _graphId,
              _orgId,
              e.Id,
              e.Name,
              source.Id,
              target.Id);
          })
          .ToList();
        var numGraphEdgeChunks = (graphEdges.Count / ChunkSize) + 1;
        var chunkRange = Enumerable.Range(0, numGraphEdgeChunks);
        await chunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
        {
          var dataChunk = graphEdges.Skip(i * ChunkSize).Take(ChunkSize);
          await _graphEdgeServer.Create(dataChunk);
        });
      }
      finally
      {
        _isAddingItems = false;
      }
    }

    private async Task AddFilteredItems()
    {
      await AddGraphItems(_table.GetSearchTerm());
    }

    private async Task AddAllItems()
    {
      await AddGraphItems();
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
