using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphItemAttributeValidator : ValidatorBase<GraphItemAttribute>, IGraphItemAttributeValidator
  {
    public GraphItemAttributeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }

}
