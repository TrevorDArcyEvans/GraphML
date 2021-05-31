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

    #region Inject

    [Inject]
    private IChartServer _chartServer { get; set; }

    [Inject]
    private ITimelineServer _timelineServer { get; set; }

    [Inject]
    private IEdgeItemAttributeDefinitionServer _edgeItemAttribDefServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private Chart[] _charts;
    private Timeline[] _timelines;

    private bool _newChartDialogIsOpen;
    private bool _newTimelineDialogIsOpen;
    private string _newItemName;
    private string _dlgNewItemName;

    private bool _deleteChartDialogIsOpen;
    private Chart _deleteChartItem;

    private bool _deleteTimelineDialogIsOpen;
    private Timeline _deleteTimelineItem;

    private EdgeItemAttributeDefinition _selIntervalAttr;
    private EdgeItemAttributeDefinition[] _intervalAttrs;

    protected override async Task OnInitializedAsync()
    {
      var intervalAttrsPage = await _edgeItemAttribDefServer.ByOwner(Guid.Parse(RepositoryManagerId), 1, int.MaxValue, null);
      _intervalAttrs = intervalAttrsPage
        .Items
        .Where(eiad => eiad.DataType == "DateTimeInterval")
        .ToArray();
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

    private async Task OkNewTimelineClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewItemName) ||
          _selIntervalAttr is null)
      {
        return;
      }

      _newItemName = _dlgNewItemName;
      _newTimelineDialogIsOpen = false;
      var newItem = await CreateNewTimeline(_newItemName);
      GotoShowTimeline(newItem);
    }

    private async Task<Chart> CreateNewChart(string itemName)
    {
      var newItem = new Chart(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName);
      var newItems = await _chartServer.Create(new[] { newItem });

      return newItems.Single();
    }

    private async Task<Timeline> CreateNewTimeline(string itemName)
    {
      var newItem = new Timeline(Guid.Parse(GraphId), Guid.Parse(OrganisationId), itemName, _selIntervalAttr.Id);
      var newItems = await _timelineServer.Create(new[] { newItem });

      return newItems.Single();
    }

    private void ConfirmDeleteChart(Chart item)
    {
      _deleteChartItem = item;
      _deleteChartDialogIsOpen = true;
    }

    private async Task DeleteChart()
    {
      _deleteChartDialogIsOpen = false;
      await _chartServer.Delete(new[] { _deleteChartItem });
      StateHasChanged();
    }

    private void ConfirmDeleteTimeline(Timeline item)
    {
      _deleteTimelineItem = item;
      _deleteTimelineDialogIsOpen = true;
    }

    private async Task DeleteTimeline()
    {
      _deleteTimelineDialogIsOpen = false;
      await _timelineServer.Delete(new[] { _deleteTimelineItem });
      StateHasChanged();
    }

    private void GotoShowChart(Chart chart)
    {
      _navMgr.NavigateTo($"/ShowChart/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{chart.Id}/{chart.Name}");
    }

    private async Task GotoShowTimeline(Timeline timeline)
    {
      var attrDefs = await _edgeItemAttribDefServer.ByIds(new[] { timeline.DateTimeIntervalAttributeDefinitionId });
      var attrDef = attrDefs.Single();
      _navMgr.NavigateTo($"/ShowTimeLine/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}/{timeline.Id}/{timeline.Name}/{attrDef.Id}/{attrDef.Name}");
    }
  }
}
