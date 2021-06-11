using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class NodeLogic : RepositoryItemLogic<Node>, INodeLogic
  {
    public NodeLogic(
      IHttpContextAccessor context,
      ILogger<NodeLogic> logger,
      INodeDatastore datastore,
      INodeValidator validator,
      INodeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
