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

    private Guid correlationId;
    private IRequest request;

    protected override async Task OnInitializedAsync()
    {
      correlationId = Guid.Parse(CorrelationId);
      request = await _resultServer.ByCorrelation(correlationId);
    }

    private RenderFragment CreateDynamicComponent() => builder =>
    {
      if (request is IClosenessRequest)
      {
        builder.OpenComponent(0, typeof(Closeness));
        builder.AddAttribute(1, nameof(Closeness.CorrelationId), correlationId);
        builder.CloseComponent();
      }

      if (request is IDegreeRequest)
      {
        builder.OpenComponent(0, typeof(Degree));
        builder.AddAttribute(1, nameof(Degree.CorrelationId), correlationId);
        builder.CloseComponent();
      }

      if (request is IBetweennessRequest)
      {
        builder.OpenComponent(0, typeof(Betweenness));
        builder.AddAttribute(1, nameof(Betweenness.CorrelationId), correlationId);
        builder.CloseComponent();
      }

      if (request is IFindShortestPathsRequest)
      {
        builder.OpenComponent(0, typeof(FindShortestPaths));
        builder.AddAttribute(1, nameof(FindShortestPaths.CorrelationId), correlationId);
        builder.CloseComponent();
      }
    };

    private void GotoBrowseResults()
    {
      _navMgr.NavigateTo($"/BrowseResults/{OrganisationId}/{OrganisationName}/{RepositoryManagerId}/{RepositoryManagerName}/{RepositoryId}/{RepositoryName}/{GraphId}/{GraphName}");
    }
  }
}
