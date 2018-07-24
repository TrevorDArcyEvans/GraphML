using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace GraphML.Logic.Filters
{
  public sealed class GraphFilter : FilterBase<Graph>, IGraphFilter
  {
    public GraphFilter(IHttpContextAccessor context) :
      base(context)
    {
    }

    protected override Graph Filter(Graph input)
    {
      throw new NotImplementedException();
    }
  }
}
