using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class GraphItemAttributeFilter : FilterBase<GraphItemAttribute>, IGraphItemAttributeFilter
  {
    public GraphItemAttributeFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore) :
      base(context, contactDatastore)
    {
    }
  }
}
