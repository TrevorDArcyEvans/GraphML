using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators.Porcelain
{
  public sealed class GraphExValidator : ValidatorBase<GraphEx>, IGraphExValidator
  {
    public GraphExValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
