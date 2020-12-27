using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class EdgeItemAttributeFilter : FilterBase<EdgeItemAttribute>, IEdgeItemAttributeFilter
  {
    public EdgeItemAttributeFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore) :
      base(context, contactDatastore)
    {
    }
  }
}
