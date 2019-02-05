using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphValidator : OwnedValidatorBase<Graph>, IGraphValidator
  {
    public GraphValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
