using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class NodeFilter : FilterBase<Node>, INodeFilter
  {
    public NodeFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
