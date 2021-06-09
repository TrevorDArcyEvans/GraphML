using System;
using System.Threading.Tasks;
using GraphML.Analysis.FindDuplicates;
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

    private Guid _findShortestPathsCorrelationId = Guid.Empty;
    private Guid _betweennessCorrelationId = Guid.Empty;
    private Guid _closenessCorrelationId = Guid.Empty;
    private Guid _degreeCorrelationId = Guid.Empty;
    private Guid _findDuplicatesCorrelationId = Guid.Empty;

    private Guid _graphId;
    private Contact _contact;

    private GraphNode[] _graphNodes;
    private GraphNode _shortestPathRootNode;
    private GraphNode _shortestPathGoalNode;

    private bool _newDialogIsOpen;
    private string _dlgNewItemName;
    private Func<Task> _analysis;

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

    private async Task SubmitShortestPath()
    {
      if (_shortestPathRootNode is null ||
          _shortestPathGoalNode is null ||
          _shortestPathRootNode.Id == _shortestPathGoalNode.Id)
      {
        return;
      }

      var req = new FindShortestPathsRequest
      {
        Description = _dlgNewItemName,
        Contact = _contact,
        GraphId = _graphId,
        RootNodeId = _shortestPathRootNode.Id,
        GoalNodeId = _shortestPathGoalNode.Id
      };
      _findShortestPathsCorrelationId = await _analysisServer.FindShortestPaths(req);
    }

    private async Task SubmitBetweenness()
    {
      var req = new BetweennessRequest
      {
        Description = _dlgNewItemName,
        Contact = _contact,
        GraphId = _graphId
      };
      _betweennessCorrelationId = await _analysisServer.Betweenness(req);
    }

    private async Task SubmitCloseness()
    {
      var req = new ClosenessRequest
      {
        Description = _dlgNewItemName,
        Contact = _contact,
        GraphId = _graphId
      };
      _closenessCorrelationId = await _analysisServer.Closeness(req);
    }

    private async Task SubmitDegree()
    {
      var req = new DegreeRequest
      {
        Description = _dlgNewItemName,
        Contact = _contact,
        GraphId = _graphId
      };
      _degreeCorrelationId = await _analysisServer.Degree(req);
    }

    private async Task SubmitFindDuplicates()
    {
      var req = new FindDuplicatesRequest()
      {
        Description = _dlgNewItemName,
        Contact = _contact,
        GraphId = _graphId,
        MinMatchingKeyLength = _selNumItems
      };
      _findDuplicatesCorrelationId = await _analysisServer.FindDuplicates(req);
    }

    private void OnRootSelectionChanged(object row)
    {
      _shortestPathRootNode = (GraphNode) row;
      StateHasChanged();
    }

    private void OnGoalSelectionChanged(object row)
    {
      _shortestPathGoalNode = (GraphNode) row;
      StateHasChanged();
    }

    private void OnNewDialog(Func<Task> analysis)
    {
      _dlgNewItemName = null;
      _newDialogIsOpen = true;
      _analysis = analysis;
    }

    private async Task OkClick()
    {
      if (string.IsNullOrWhiteSpace(_dlgNewItemName))
      {
        return;
      }

      _newDialogIsOpen = false;
      await _analysis();
      _analysis = null;
    }

    protected override async void OnInitialized()
    {
      base.OnInitialized();

      var email = _context.Email();
      _contact = await _contactServer.ByEmail(email);
      _graphId = Guid.Parse(GraphId);
    }

    private void GotoListItems()
    {
      _navMgr.NavigateTo($"/ListItems/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
