using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace GraphML.Logic.Filters
{
  public sealed class EdgeFilter : FilterBase<Edge>, IEdgeFilter
  {
    public EdgeFilter(IHttpContextAccessor context) :
      base(context)
    {
    }

    protected override Edge Filter(Edge input)
    {
      throw new NotImplementedException();
    }
  }
}
