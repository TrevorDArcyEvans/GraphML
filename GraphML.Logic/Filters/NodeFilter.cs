using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace GraphML.Logic.Filters
{
  public sealed class NodeFilter : FilterBase<Node>, INodeFilter
  {
    public NodeFilter(IHttpContextAccessor context) :
      base(context)
    {
    }

    protected override Node Filter(Node input)
    {
      throw new NotImplementedException();
    }
  }
}
