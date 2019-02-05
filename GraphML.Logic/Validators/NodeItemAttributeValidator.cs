using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class NodeItemAttributeValidator : OwnedValidatorBase<NodeItemAttribute>, INodeItemAttributeValidator
  {
    public NodeItemAttributeValidator(IHttpContextAccessor context) :
      base(context)
    {
    }
  }

}
