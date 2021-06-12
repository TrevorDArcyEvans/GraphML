using System;
using System.Threading.Tasks;
using GraphML.Interfaces.Server;
using GraphML.UI.Web.Widgets;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Web.Pages
{
  public partial class AddGraphNodes
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

    private Node[] _data;
    private MatTableEx<Node> _table;

    private Guid _graphId;

    private bool _addAllDialogIsOpen;
    private bool _isAddingItems;

    protected override void OnInitialized()
    {
      _graphId = Guid.Parse(GraphId);
    }

    private async Task AddGraphItems(string searchTerm = "")
    {
      try
      {
        _addAllDialogIsOpen = false;
        _isAddingItems = true;

        // force a delay so spinner is rendered
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        await _graphNodeServer.AddByFilter(_graphId, searchTerm);
      }
      finally
      {
        _isAddingItems = false;
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
