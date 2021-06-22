using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphML.Analysis;
using GraphML.Analysis.RankedShortestPath;
using GraphML.Interfaces.Server;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using MoreLinq;
using QuikGraph;

namespace GraphML.UI.Web.Pages.Visualisations
{
  public partial class FindShortestPaths
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

    [Parameter]
    public string CorrelationId { get; set; }

    #endregion

    #region Inject

    [Inject]
    public IResultServer _resultServer { get; set; }

    [Inject]
    public IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    public IGraphEdgeServer _graphEdgeServer { get; set; }

    [Inject]
    public IChartNodeServer _chartNodeServer { get; set; }

    [Inject]
    public IChartEdgeServer _chartEdgeServer { get; set; }

    [Inject]
    public IChartServer _chartServer { get; set; }

    [Inject]
    public NavigationManager _navMgr { get; set; }

    #endregion

    private List<FindShortestPathGraphResult> _results = new();
    private FindShortestPathGraphResult[] _displayResults;

    private bool _newChartDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private int _selNumItems = 10;
    private readonly int[] _numItems = Enumerable.Range(1, 10).ToArray();

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

      var genRes = await _resultServer.Retrieve(Guid.Parse(CorrelationId));
      var snaResult = (FindShortestPathsResults<IdentifiableEdge<Guid>>) genRes;
      var snaResults = snaResult.Result.ToList();
      foreach (var res in snaResults)
      {
        var graphEdgeIds = res.Path.Select(edge => edge.Id);
        var graphEdges = await _graphEdgeServer.ByIds(graphEdgeIds);
        var graphRes = new FindShortestPathGraphResult
        {
          Path = graphEdges,
          Cost = res.Cost
        };
        _results.Add(graphRes);
      }

      _displayResults = _results.ToArray();
    }

    private void SortData(MatSortChangedEvent sort)
    {
      _displayResults = _results.ToArray();

      if (!(sort == null ||
            sort.Direction == MatSortDirection.None ||
            string.IsNullOrEmpty(sort.SortId)))
      {
        Comparison<double> comparison = null;

        switch (sort.SortId)
        {
          case nameof(FindShortestPathsResult<Edge<Guid>>.Cost):
            comparison = (s1, s2) => s1.CompareTo(s2);
            break;
          default:
            throw new ArgumentOutOfRangeException($"Unknown sort:  {sort.SortId}");
        }

        if (sort.Direction == MatSortDirection.Desc)
        {
          Array.Sort(_displayResults, (s1, s2) => -1 * comparison(s1.Cost, s2.Cost));
        }
        else
        {
          Array.Sort(_displayResults, (s1, s2) => comparison(s1.Cost, s2.Cost));
        }
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
      // create Chart
      var newItem = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _chartServer.Create(new[] { newItem });
      var newChart = newItems.Single();

      // create ChartNodes + ChartEdges
      var graphEdges = _displayResults
        .Take(_selNumItems)
        .SelectMany(res => res.Path)
        .DistinctBy(ge => ge.Id)
        .ToList();
      var graphNodeIds = graphEdges
        .SelectMany(ge => new[] { ge.GraphSourceId, ge.GraphTargetId })
        .Distinct();
      var graphNodes = await _graphNodeServer.ByIds(graphNodeIds);
      var chartNodes = graphNodes
        .Select(gn => new ChartNode(newChart.Id, gn.OrganisationId, gn.Id, gn.Name))
        .ToList();
      var chartEdges = graphEdges
        .Select(ge =>
          new ChartEdge(
            newChart.Id,
            ge.OrganisationId,
            ge.Id,
            ge.Name,
            chartNodes.Single(cn => cn.GraphItemId == ge.GraphSourceId).Id,
            chartNodes.Single(cn => cn.GraphItemId == ge.GraphTargetId).Id));
      _ = await _chartNodeServer.Create(chartNodes);
      _ = await _chartEdgeServer.Create(chartEdges);

      return newChart;
    }

    private void GotoShowChart(Chart chart)
    {
      _navMgr.NavigateTo($"/ShowChart/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{chart.Id}/{chart.Name}");
    }
  }
}
