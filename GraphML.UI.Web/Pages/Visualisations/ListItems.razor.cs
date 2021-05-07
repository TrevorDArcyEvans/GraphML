using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTable;
using GraphML.Interfaces.Server;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace GraphML.UI.Web.Pages.Visualisations
{
  public partial class ListItems
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
    private IGraphNodeServer _graphNodeServer { get; set; }

    [Inject]
    private IConfiguration _config { get; set; }

    [Inject]
    private NavigationManager _navMgr { get; set; }

    private List<GraphNode> _data;
    private Table<GraphNode> _table;

    private Guid _graphId;

    protected override async Task OnInitializedAsync()
    {
      _graphId = Guid.Parse(GraphId);

      // get GraphNodes already in Graph
      var allGraphNodesPage = await _graphNodeServer.ByOwner(_graphId, 0, int.MaxValue, null);
      var allGraphNodes = allGraphNodesPage.Items;
      _data = allGraphNodes.ToList();
    }

    private void GotoBrowseAnalyses()
    {
      _navMgr.NavigateTo($"/BrowseAnalyses/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
