using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class GraphNodeLogic : GraphItemLogicBase<GraphNode>, IGraphNodeLogic
  {
    public GraphNodeLogic(
      IHttpContextAccessor context,
      ILogger<GraphNodeLogic> logger,
      IGraphNodeDatastore datastore,
      IGraphNodeValidator validator,
      IGraphNodeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
