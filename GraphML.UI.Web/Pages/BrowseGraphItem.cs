using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public class BrowseGraphItem : ComponentBase
  {
    [Inject]
    private NavigationManager UriHelper { get; set; }

    [Parameter]
    public string GraphName { get; set; }

    [Parameter]
    public string GraphId { get; set; }

    protected void BrowseGraph()
    {
      UriHelper.NavigateTo($"/BrowseGraph/{GraphName}/{GraphId}");
    }
  }
}
