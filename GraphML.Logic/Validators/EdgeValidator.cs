using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class EdgeValidator : ValidatorBase<Edge>, IEdgeValidator
  {
    public EdgeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
