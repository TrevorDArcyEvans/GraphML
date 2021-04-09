using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class EdgeItemAttributeDefinitionFilter : FilterBase<EdgeItemAttributeDefinition>, IEdgeItemAttributeDefinitionFilter
  {
    public EdgeItemAttributeDefinitionFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
