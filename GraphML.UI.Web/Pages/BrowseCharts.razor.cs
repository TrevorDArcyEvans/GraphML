using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseCharts
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

    private Chart[] _charts;

    private bool _newDialogIsOpen;
    private string _newChartName;
    private string _dlgNewChartName;

    private bool _deleteDialogIsOpen;
    private Chart _deleteChart;

    private void NewDialog()
    {
      _dlgNewChartName = null;
      _newDialogIsOpen = true;
    }

    private async Task OkClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewChartName))
      {
        return;
      }

      _newChartName = _dlgNewChartName;
      _newDialogIsOpen = false;
      var newChart = await CreateNewChart(_newChartName);
      GotoShowChart(newChart);
    }

    private async Task<Chart> CreateNewChart(string chartName)
    {
      var newChart = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), chartName);
      var newCharts = await _chartServer.Create(new[] { newChart });

      return newCharts.Single();
    }

    private void ConfirmDeleteChart(Chart chart)
    {
      _deleteChart = chart;
      _deleteDialogIsOpen = true;
    }

    private async Task DeleteChart()
    {
      _deleteDialogIsOpen = false;
      await _chartServer.Delete(new[] { _deleteChart });
      StateHasChanged();
    }

    private void GotoShowChart(Chart chart)
    {
      _navMgr.NavigateTo($"/ShowChart/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{chart.Id}/{chart.Name}");
    }

    private void GotoBrowseGraphs()
    {
      _navMgr.NavigateTo($"/BrowseGraphs/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}");
    }
  }
}
