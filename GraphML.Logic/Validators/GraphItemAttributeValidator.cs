using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphItemAttributeValidator : OwnedValidatorBase<GraphItemAttribute>, IGraphItemAttributeValidator
  {
    public GraphItemAttributeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }

}
