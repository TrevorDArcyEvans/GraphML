using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class EdgeFilter : FilterBase<Edge>, IEdgeFilter
  {
    public EdgeFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
