using System;
using System.Threading.Tasks;
using GraphML.Interfaces.Server;
using GraphML.UI.Web.Widgets;
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
    private IGraphEdgeServer _graphEdgeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    #endregion

    private Edge[] _data;
    private MatTableEx<Edge> _table;
    
    private Guid _graphId;

    private bool _addAllDialogIsOpen;
    private bool _isBusy;

    protected override void OnInitialized()
    {
      _graphId = Guid.Parse(GraphId);
    }

    private async Task AddGraphItems(string searchTerm = "")
    {
      try
      {
        _addAllDialogIsOpen = false;
        _isBusy = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        await _graphEdgeServer.AddByFilter(_graphId, searchTerm);
      }
      finally
      {
        _isBusy = false;
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
  }
}

