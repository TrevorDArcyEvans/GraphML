using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class EdgeItemAttributeValidator : ValidatorBase<EdgeItemAttribute>, IEdgeItemAttributeValidator
  {
    public EdgeItemAttributeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }

}
