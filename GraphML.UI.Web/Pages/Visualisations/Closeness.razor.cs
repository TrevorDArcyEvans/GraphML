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
  public partial class Closeness
  {
    [Parameter]
    public Guid CorrelationId { get; set; }

    #region Inject

    [Inject]
    private IResultServer _resultServer { get; set; }

    [Inject]
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IChartServer _chartServer { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private IEnumerable<SnaClosenessNode> _results;
    private SnaClosenessNode[] _graphNodes;

    private string OrganisationName { get; set; }
    private string OrganisationId { get; set; }
    private string RepositoryManagerName { get; set; }
    private string RepositoryManagerId { get; set; }
    private string RepositoryName { get; set; }
    private string RepositoryId { get; set; }
    private string GraphName { get; set; }
    private string GraphId { get; set; }

    private bool _newChartDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private int _selNumItems = 10;
    private readonly int[] _numItems = new int[]
    {
      1,
      2,
      3,
      4,
      5,
      6,
      7,
      8,
      9,
      10
    };

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

      var genRes = await _resultServer.Retrieve(CorrelationId);
      var snaResult = (ClosenessResult<Guid>) genRes;
      var snaResults = snaResult.Result.ToList();
      var graphNodeIds = snaResults.Select(res => res.Vertex);
      var graphNodes = await _graphNodeServer.ByIds(graphNodeIds);
      var results = graphNodes.Select(gn =>
      {
        return new SnaClosenessNode
        {
          GraphNode = gn,
          Closeness = snaResults.Single(res => res.Vertex == gn.Id).Closeness
        };
      });
      _graphNodes = results.ToArray();
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
          case nameof(ClosenessVertexResult<Guid>.Closeness):
            comparison = (s1, s2) => s1.CompareTo(s2);
            break;
          default:
            throw new ArgumentOutOfRangeException($"Unknown sort:  {sort.SortId}");
        }

        if (sort.Direction == MatSortDirection.Desc)
        {
          Array.Sort(_graphNodes, (s1, s2) => -1 * comparison(s1.Closeness, s2.Closeness));
        }
        else
        {
          Array.Sort(_graphNodes, (s1, s2) => comparison(s1.Closeness, s2.Closeness));
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
      // TODO   take first _graphNodes and get OrganisationId, OrganisationName, GraphId, GraphName et al
      var newItem = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _chartServer.Create(new[] { newItem });
      
      // TODO   add top n items to chart

      return newItems.Single();
    }

    private void GotoShowChart(Chart chart)
    {
      _navMgr.NavigateTo($"/ShowChart/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{chart.Id}/{chart.Name}");
    }
  }
}
