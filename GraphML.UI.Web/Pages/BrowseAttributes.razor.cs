using Microsoft.AspNetCore.Components;

namespace GraphML.UI.Web.Pages
{
  public partial class BrowseAttributes
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

    #endregion

    private RepositoryItemAttributeDefinition[] _repoAttrDefs;
    private GraphItemAttributeDefinition[] _graphAttrDefs;
    private NodeItemAttributeDefinition[] _nodeAttrDefs;
    private EdgeItemAttributeDefinition[] _edgeAttrDefs;

    private void GotoBrowseRepositoryManagers()
    {
      _navMgr.NavigateTo($"/BrowseRepositoryManagers/{OrganisationId}/{OrganisationName}");
    }
  }
}
