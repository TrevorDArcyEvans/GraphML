using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTable;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Web.Pages
{
  public partial class AddGraphItems
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
    private INodeServer _nodeServer { get; set; }

    [Inject]
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    private List<Node> _data;
    private Table<Node> _table;

    private Guid _graphId;
    private Guid _orgid;

    protected override async Task OnInitializedAsync()
    {
      _graphId = Guid.Parse(GraphId);
      _orgid = Guid.Parse(OrganisationId);

      // TODO   check to see what is already in Graph and remove those GraphNodes
      //        from available Nodes
      var dataPage = await _nodeServer.ByOwner(Guid.Parse(RepositoryId), 0, int.MaxValue, null);
      _data = dataPage.Items.ToList();
    }

    private async Task AddSelectedGraphItems()
    {
      var selItems = _table.SelectedItems;
      var graphNodes = selItems.Select(n => new GraphNode(_graphId, _orgid, n.Id, n.Name));
      await _graphNodeServer.Create(graphNodes);

      // successfully created new GraphNodes, so remove
      // underlying Nodes from available selection
      selItems.ForEach(item => _data.Remove(item));
    }

    private void GotoBrowseGraphItems()
    {
      _navMgr.NavigateTo($"/BrowseGraphItems/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
