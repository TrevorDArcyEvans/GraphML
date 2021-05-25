using System;
using System.Threading.Tasks;
using GraphML.Interfaces;
using GraphML.UI.Web.Pages.Visualisations;
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
      var index = 0;
      switch (_request)
      {
        case IClosenessRequest:
          builder.OpenComponent(index++, typeof(Closeness));
          break;
        case IDegreeRequest:
          builder.OpenComponent(index++, typeof(Degree));
          break;
        case IBetweennessRequest:
          builder.OpenComponent(index++, typeof(Betweenness));
          break;
        case IFindShortestPathsRequest:
          builder.OpenComponent(index++, typeof(FindShortestPaths));
          break;
        default:
          throw new ArgumentOutOfRangeException($"Unknown request:  {_request}");
      }

      builder.AddAttribute(index++, nameof(OrganisationName), OrganisationName);
      builder.AddAttribute(index++, nameof(OrganisationId), OrganisationId);
      builder.AddAttribute(index++, nameof(RepositoryManagerName), RepositoryManagerName);
      builder.AddAttribute(index++, nameof(RepositoryManagerId), RepositoryManagerId);
      builder.AddAttribute(index++, nameof(RepositoryName), RepositoryName);
      builder.AddAttribute(index++, nameof(RepositoryId), RepositoryId);
      builder.AddAttribute(index++, nameof(GraphName), GraphName);
      builder.AddAttribute(index++, nameof(GraphId), GraphId);
      builder.AddAttribute(index++, nameof(CorrelationId), CorrelationId);

      builder.CloseComponent();
    };
  }
}
