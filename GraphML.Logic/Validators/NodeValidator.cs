using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class NodeValidator : OwnedValidatorBase<Node>, INodeValidator
  {
    public NodeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
