using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class ItemAttributeValidator : ValidatorBase<ItemAttribute>, IItemAttributeValidator
  {
    public ItemAttributeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
