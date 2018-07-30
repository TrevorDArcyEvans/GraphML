using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters.Porcelain
{
  public sealed class GraphExFilter : FilterBase<GraphEx>, IGraphExFilter
  {
    public GraphExFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
