using System;
using System.Threading.Tasks;
using GraphML.Analysis.RankedShortestPath;
using GraphML.Analysis.SNA.Centrality;
using GraphML.Common;
using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseAnalyses
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

    public Guid FindShortestPathsCorrelationId { get; set; } = Guid.Empty;
    public Guid BetweennessCorrelationId { get; set; } = Guid.Empty;
    public Guid ClosenessCorrelationId { get; set; } = Guid.Empty;
    public Guid DegreeCorrelationId { get; set; } = Guid.Empty;

    private Contact _contact;

    private async Task SubmitShortestPath()
    {
      // TODO     RootNodeId
      // TODO     GoalNodeId
      var req = new FindShortestPathsRequest
      {
        Contact = _contact,
        GraphId = Guid.Parse(GraphId),
        RootNodeId = Guid.NewGuid(),
        GoalNodeId = Guid.NewGuid()
      };
      FindShortestPathsCorrelationId = await _analysisServer.FindShortestPaths(req);
    }

    private async Task SubmitBetweenness()
    {
      var req = new BetweennessRequest
      {
        Contact = _contact,
        GraphId = Guid.Parse(GraphId)
      };
      BetweennessCorrelationId = await _analysisServer.Betweenness(req);
    }

    private async Task SubmitCloseness()
    {
      var req = new ClosenessRequest
      {
        Contact = _contact,
        GraphId = Guid.Parse(GraphId)
      };
      ClosenessCorrelationId = await _analysisServer.Closeness(req);
    }

    private async Task SubmitDegree()
    {
      var req = new DegreeRequest
      {
        Contact = _contact,
        GraphId = Guid.Parse(GraphId)
      };
      DegreeCorrelationId = await _analysisServer.Degree(req);
    }

    protected override async void OnInitialized()
    {
      base.OnInitialized();

      var email = _context.Email();
      _contact = await _contactServer.ByEmail(email);
    }

    private void GotoBrowseGraphs()
    {
      _navMgr.NavigateTo($"/BrowseGraphs/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}");
    }
  }
}
