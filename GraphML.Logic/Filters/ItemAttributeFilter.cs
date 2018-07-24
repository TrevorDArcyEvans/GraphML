using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace GraphML.Logic.Filters
{
  public sealed class ItemAttributeFilter : FilterBase<ItemAttribute>, IItemAttributeFilter
  {
    public ItemAttributeFilter(IHttpContextAccessor context) :
      base(context)
    {
    }

    protected override ItemAttribute Filter(ItemAttribute input)
    {
      throw new NotImplementedException();
    }
  }
}
