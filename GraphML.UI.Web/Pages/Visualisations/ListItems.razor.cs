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
    public IChartNodeServer _chartNodeServer { get; set; }

    [Inject]
    public IChartServer _chartServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private const int ChunkSize = 1000;
    private const int DegreeOfParallelism = 10;

    private List<GraphNode> _data;
    private Table<GraphNode> _table;
    private readonly List<int> _pageSizes = new List<int>(new[] { 5, 10, 25, 50, 100 });

    private Guid _graphId;
    
    private bool _newChartDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private int _selNumItems = 100;
    private readonly int[] _numItems = new int[]
    {
      10,
      25,
      50,
      100,
      250,
      500,
      1000
    };

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
        _graphId = Guid.Parse(GraphId);
        await LoadData();
      }
    }

    private async Task LoadData()
    {
      // get GraphNodes already in Graph
      var allGraphNodesCount = await _graphNodeServer.Count(_graphId);
      _data = new List<GraphNode>(allGraphNodesCount);
      var allGraphNodes = new ConcurrentBag<GraphNode>();
      var numChunks = (allGraphNodesCount / ChunkSize) + 1;
      var chunkRange = Enumerable.Range(0, numChunks);
      await chunkRange.ParallelForEachAsync(DegreeOfParallelism, async i =>
      {
        var allGraphNodesPage = await _graphNodeServer.ByOwner(_graphId, i + 1, ChunkSize, null);
        allGraphNodesPage.Items
          .ToList()
          .ForEach(gn => allGraphNodes.Add(gn));
      });
      _data.AddRange(allGraphNodes);

      StateHasChanged();
    }

    private async Task OnTakeAction(ListItemsAction itemsAction)
    {
      var oldPageSize = _table.PageSize;
      try
      {
        // Table.Filtered items only returns *displayed* items on current page,
        // so we force display all (filtered) items
        await _table.SetPageSizeAsync(int.MaxValue);

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
      finally
      {
        await _table.SetPageSizeAsync(oldPageSize);
      }
    }

    private async Task OkNewChartClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewItemName))
      {
        return;
      }

      _newItemName = _dlgNewItemName;
      _newChartDialogIsOpen = false;
      var newItem = await CreateNewChart(_newItemName);
      GotoShowChart(newItem);
    }

    private async Task<Chart> CreateNewChart(string itemName)
    {
      var newItem = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _chartServer.Create(new[] { newItem });
      var newChart = newItems.Single();
      
      var chartNodes = _data
        .Take(_selNumItems)
        .Select(gn => new ChartNode(newChart.Id, gn.OrganisationId, gn.Id, gn.Name));
      _ = await _chartNodeServer.Create(chartNodes);
      
      return newChart;
    }

    private void GotoShowChart(Chart chart)
    {
      _navMgr.NavigateTo($"/ShowChart/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{chart.Id}/{chart.Name}");
    }
  }
}
