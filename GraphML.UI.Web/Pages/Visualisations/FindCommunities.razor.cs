using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphML.Analysis.FindCommunities;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages.Visualisations
{
  public partial class FindCommunities
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

    // give a small sample of members as there could be thousands!
    private const int SampleSize = 10;

    private FindCommunitiesResult _result;
    private List<List<Guid>> _data;
    private readonly List<GraphNode> _sampleGraphNodes = new();

    private bool _newChartDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private int _selNumItems = 10;
    private readonly int[] _numItems = Enumerable.Range(1, 10).ToArray();

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

      var genRes = await _resultServer.Retrieve(Guid.Parse(CorrelationId));
      _result = (FindCommunitiesResult) genRes;
      var datas = _result.Result.OrderByDescending(x => x.Count).ToList();
      var sampleIds = datas.SelectMany(d => d.Take(SampleSize));
      var samples = await _graphNodeServer.ByIds(sampleIds);

      _sampleGraphNodes.AddRange(samples);
      _data = datas;
    }

    private async Task OkNewChartClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewItemName))
      {
        return;
      }

      _newItemName = _dlgNewItemName;
      _newChartDialogIsOpen = false;
      var newItem = await CreateNewChart(_newItemName, _selNumItems);
      GotoShowChart(newItem);
    }

    private async Task<Chart> CreateNewChart(string itemName, int selNumItems)
    {
      var newItem = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _chartServer.Create(new[] { newItem });
      var newChart = newItems.Single();
      var communities = _data.Take(selNumItems);
      foreach (var community in communities)
      {
        var graphNodes = (await _graphNodeServer.ByIds((community))).ToList();
        var graphNodeIds = graphNodes.Select(gn => gn.Id).ToHashSet();
        var graphEdgesPage = await _graphEdgeServer.ByNodeIds(graphNodeIds, 0, int.MaxValue, null);
        var graphEdges = graphEdgesPage.Items
          .Where(ge => graphNodeIds.Contains(ge.GraphSourceId) && graphNodeIds.Contains(ge.GraphTargetId));
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
      }

      return newChart;
    }

    private void GotoShowChart(Chart chart)
    {
      _navMgr.NavigateTo($"/ShowChart/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{chart.Id}/{chart.Name}");
    }
  }
}
