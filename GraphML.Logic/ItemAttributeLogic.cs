using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ItemAttributeLogic : LogicBase<ItemAttribute>, IItemAttributeLogic
  {
    public ItemAttributeLogic(
      IHttpContextAccessor context,
      IItemAttributeDatastore datastore,
      IItemAttributeValidator validator,
      IItemAttributeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
