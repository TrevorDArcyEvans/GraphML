using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class NodeItemAttributeLogic : LogicBase<NodeItemAttribute>, INodeItemAttributeLogic
  {
    public NodeItemAttributeLogic(
      IHttpContextAccessor context,
      INodeItemAttributeDatastore datastore,
      INodeItemAttributeValidator validator,
      INodeItemAttributeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
