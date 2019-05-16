using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public class BrowseGraphItem : ComponentBase
  {
    [Inject]
    private IUriHelper UriHelper { get; set; }

    [Parameter]
    protected string GraphName { get; private set; }

    [Parameter]
    protected string GraphId { get; private set; }

    protected void BrowseGraph()
    {
      UriHelper.NavigateTo($"/BrowseGraph/{GraphName}/{GraphId}");
    }
  }
}
