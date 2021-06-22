using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphML.Analysis.FindCommunities;
using GraphML.Analysis.FindDuplicates;
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
    public IChartNodeServer _chartNodeServer { get; set; }

    [Inject]
    public IChartServer _chartServer { get; set; }

    [Inject]
    public NavigationManager _navMgr { get; set; }

    #endregion

    private FindCommunitiesResult _result;
    private List<List<Guid>> _data;

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

      var genRes = await _resultServer.Retrieve(Guid.Parse(CorrelationId));
      _result = (FindCommunitiesResult) genRes;
      _data = _result.Result.OrderBy(x => x.Count()).ToList();
    }
  }
}
