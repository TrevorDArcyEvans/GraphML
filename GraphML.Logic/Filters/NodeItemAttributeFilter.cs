using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class NodeItemAttributeFilter : FilterBase<NodeItemAttribute>, INodeItemAttributeFilter
  {
    public NodeItemAttributeFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
