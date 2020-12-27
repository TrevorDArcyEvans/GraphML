using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class GraphNodeFilter : FilterBase<GraphNode>, IGraphNodeFilter
  {
    public GraphNodeFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore) :
      base(context, contactDatastore)
    {
    }
  }
}
