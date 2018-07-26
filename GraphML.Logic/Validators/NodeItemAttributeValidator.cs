using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class NodeItemAttributeValidator : ValidatorBase<NodeItemAttribute>, INodeItemAttributeValidator
  {
    public NodeItemAttributeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }

}
