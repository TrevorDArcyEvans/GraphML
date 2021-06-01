using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTable;
using GraphML.Interfaces.Server;
using GraphML.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Web.Pages.Visualisations
{
  public partial class ListItems
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
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private const int ChunkSize = 1000;
    private const int DegreeofParallelism = 10;

    private List<GraphNode> _data;
    private Table<GraphNode> _table;

    private Guid _graphId;

    private int _itemsAction;

    public enum ListItemsAction
    {
      TakeHalf,
      TakeQuarter,
      TakeSelected
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (firstRender)
      {
        await LoadData();
      }
    }

    private async Task LoadData()
    {
      _graphId = Guid.Parse(GraphId);

      var lockObj = new object();

      // get GraphNodes already in Graph
      var allGraphNodesCount = await _graphNodeServer.Count(_graphId);
      _data = new List<GraphNode>(allGraphNodesCount);
      var numChunks = (allGraphNodesCount / ChunkSize) + 1;
      var chunkRange = Enumerable.Range(0, numChunks);
      await chunkRange.ParallelForEachAsync(DegreeofParallelism, async i =>
      {
        var allGraphNodesPage = await _graphNodeServer.ByOwner(_graphId, i + 1, ChunkSize, null);
        var allGraphNodes = allGraphNodesPage.Items;
        lock (lockObj)
        {
          _data.AddRange(allGraphNodes);
        }
      });

      StateHasChanged();
    }

    private void OnTakeAction(ListItemsAction itemsAction)
    {
      var filtCount = _table.FilteredItems.Count();

      switch (itemsAction)
      {
        case ListItemsAction.TakeHalf:
          _data = _table.FilteredItems.Take(filtCount / 2 + 1).ToList();
          break;
        case ListItemsAction.TakeQuarter:
          _data = _table.FilteredItems.Take(filtCount / 4 + 1).ToList();
          break;
        case ListItemsAction.TakeSelected:
          _data = _table.SelectedItems.ToList();
          break;
        default:
          throw new ArgumentOutOfRangeException($"Unknown action:  {itemsAction}");
      }
    }
  }
}
