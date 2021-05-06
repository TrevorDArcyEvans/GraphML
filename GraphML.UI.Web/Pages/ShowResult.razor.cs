using System;
using System.Threading.Tasks;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class ShowResult
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

    private Guid _correlationId;
    private IRequest _request;

    protected override async Task OnInitializedAsync()
    {
      _correlationId = Guid.Parse(CorrelationId);
      _request = await _resultServer.ByCorrelation(_correlationId);
    }

    private RenderFragment CreateDynamicComponent() => builder =>
    {
      // TODO   use external config to map [result] --> [visualiser]
      switch (_request)
      {
        case IClosenessRequest:
          builder.OpenComponent(0, typeof(Closeness));
          builder.AddAttribute(1, nameof(Closeness.CorrelationId), _correlationId);
          builder.CloseComponent();
          break;
        case IDegreeRequest:
          builder.OpenComponent(0, typeof(Degree));
          builder.AddAttribute(1, nameof(Degree.CorrelationId), _correlationId);
          builder.CloseComponent();
          break;
        case IBetweennessRequest:
          builder.OpenComponent(0, typeof(Betweenness));
          builder.AddAttribute(1, nameof(Betweenness.CorrelationId), _correlationId);
          builder.CloseComponent();
          break;
        case IFindShortestPathsRequest:
          builder.OpenComponent(0, typeof(FindShortestPaths));
          builder.AddAttribute(1, nameof(FindShortestPaths.CorrelationId), _correlationId);
          builder.CloseComponent();
          break;
        default:
          throw new ArgumentOutOfRangeException($"Unknown request:  {_request}");
      }
    };

    private void GotoBrowseResults()
    {
      _navMgr.NavigateTo($"/BrowseResults/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
