using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphML.Analysis.SNA.Centrality;
using GraphML.Interfaces.Server;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages.Visualisations
{
  public partial class Betweenness
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
    public IChartNodeServer _chartNodeServer { get; set; }

    [Inject]
    public IChartServer _chartServer { get; set; }

    [Inject]
    public NavigationManager _navMgr { get; set; }

    #endregion

    private IEnumerable<SnaBetweennessNode> _results;
    private SnaBetweennessNode[] _graphNodes;

    private bool _newChartDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private int _selNumItems = 10;
    private readonly int[] _numItems = Enumerable.Range(1, 10).ToArray();

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

      var genRes = await _resultServer.Retrieve(Guid.Parse(CorrelationId));
      var snaResult = (BetweennessResult<Guid>) genRes;
      var snaResults = snaResult.Result.ToList();
      var graphNodeIds = snaResults.Select(res => res.Vertex);
      var graphNodes = await _graphNodeServer.ByIds(graphNodeIds);
      _results = graphNodes.Select(gn =>
      {
        return new SnaBetweennessNode
        {
          GraphNode = gn,
          Betweenness = snaResults.Single(res => res.Vertex == gn.Id).Betweenness
        };
      });

      _graphNodes = _results.ToArray();
    }

    private void SortData(MatSortChangedEvent sort)
    {
      _graphNodes = _results.ToArray();

      if (!(sort == null ||
            sort.Direction == MatSortDirection.None ||
            string.IsNullOrEmpty(sort.SortId)))
      {
        Comparison<double> comparison = null;

        switch (sort.SortId)
        {
          case nameof(SnaBetweennessNode.Betweenness):
            comparison = (s1, s2) => s1.CompareTo(s2);
            break;
          default:
            throw new ArgumentOutOfRangeException($"Unknown sort:  {sort.SortId}");
        }

        if (sort.Direction == MatSortDirection.Desc)
        {
          Array.Sort(_graphNodes, (s1, s2) => -1 * comparison(s1.Betweenness, s2.Betweenness));
        }
        else
        {
          Array.Sort(_graphNodes, (s1, s2) => comparison(s1.Betweenness, s2.Betweenness));
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
      var newItem = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _chartServer.Create(new[] { newItem });
      var newChart = newItems.Single();

      var chartNodes = _graphNodes
        .Take(_selNumItems)
        .Select(snaNode => new ChartNode(newChart.Id, snaNode.GraphNode.OrganisationId, snaNode.GraphNode.Id, snaNode.GraphNode.Name));
      _ = await _chartNodeServer.Create(chartNodes);

      return newChart;
    }

    private void GotoShowChart(Chart chart)
    {
      _navMgr.NavigateTo($"/ShowChart/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{chart.Id}/{chart.Name}");
    }
  }
}
