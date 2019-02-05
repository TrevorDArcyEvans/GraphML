using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class NodeLogic : OwnedLogicBase<Node>, INodeLogic
  {
    public NodeLogic(
      IHttpContextAccessor context,
      INodeDatastore datastore,
      INodeValidator validator,
      INodeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
