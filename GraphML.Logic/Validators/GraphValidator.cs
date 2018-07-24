using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphValidator : ValidatorBase<Graph>, IGraphValidator
  {
    public GraphValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
