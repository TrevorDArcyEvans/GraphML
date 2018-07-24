using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class ItemAttributeFilter : FilterBase<ItemAttribute>, IItemAttributeFilter
  {
    public ItemAttributeFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
