using System;
using System.Linq;
using System.Threading.Tasks;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

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

    [Inject]
    private IChartServer _chartServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }
    
    private Chart[] _charts;

    private bool _newChartDialogIsOpen;
    private bool _newTimelineDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private bool _deleteDialogIsOpen;
    private Chart _deleteItem;

    private EdgeItemAttributeDefinition _selIntervalAttr;

    // TODO   retrieve DateTimeInterval types from IEdgeItemAttributeDefinitionServer
    private EdgeItemAttributeDefinition[] _intervalAttrs = new[]
    {
      new EdgeItemAttributeDefinition()
      {
        Name = "aaa"
      },
      new EdgeItemAttributeDefinition()
      {
        Name = "bbb"
      },
      new EdgeItemAttributeDefinition()
      {
        Name = "ccc"
      },
    };

    private async Task OkNewChartClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewItemName))
      {
        return;
      }

      _newItemName = _dlgNewItemName;
      _newChartDialogIsOpen = false;
      var newItem = await CreateNewItem(_newItemName);
      GotoShowChart(newItem);
    }

    private async Task OkNewTimelineClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewItemName))
      {
        return;
      }

      _newItemName = _dlgNewItemName;
      _newTimelineDialogIsOpen = false;
     var newItem = await CreateNewTimeline(_newItemName);
      GotoShowTimeline(newItem);
    }

    private async Task<Chart> CreateNewItem(string itemName)
    {
      var newItem = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _chartServer.Create(new[] { newItem });

      return newItems.Single();
    }

    private async Task<Chart> CreateNewTimeline(string itemName)
    {
      var newItem = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName);
      // TODO   create timeline in db
      //var newItems = await _chartServer.Create(new[] { newItem });

      //return newItems.Single();
      return newItem;
    }

    private void ConfirmDelete(Chart item)
    {
      _deleteItem = item;
      _deleteDialogIsOpen = true;
    }

    private async Task Delete()
    {
      _deleteDialogIsOpen = false;
      await _chartServer.Delete(new[] { _deleteItem });
      StateHasChanged();
    }

    private void GotoShowChart(Chart chart)
    {
      _navMgr.NavigateTo($"/ShowChart/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{chart.Id}/{chart.Name}");
    }

    private void GotoShowTimeline(Chart timeLine)
    {
      _navMgr.NavigateTo($"/ShowTimeLine/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{timeLine.Id}/{timeLine.Name}");
    }
  }
}
