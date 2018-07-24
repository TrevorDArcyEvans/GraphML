using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class NodeValidator : ValidatorBase<Node>, INodeValidator
  {
    public NodeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
