using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class NodeItemAttributeLogic : OwnedLogicBase<NodeItemAttribute>, INodeItemAttributeLogic
  {
    public NodeItemAttributeLogic(
      IHttpContextAccessor context,
      ILogger<NodeItemAttributeLogic> logger,
      INodeItemAttributeDatastore datastore,
      INodeItemAttributeValidator validator,
      INodeItemAttributeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
