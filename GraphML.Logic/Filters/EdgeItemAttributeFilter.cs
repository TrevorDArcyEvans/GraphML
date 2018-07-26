using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class EdgeItemAttributeFilter : FilterBase<EdgeItemAttribute>, IEdgeItemAttributeFilter
  {
    public EdgeItemAttributeFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
