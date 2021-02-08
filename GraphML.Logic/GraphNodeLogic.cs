using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class GraphNodeLogic : OwnedLogicBase<GraphNode>, IGraphNodeLogic
  {
    public GraphNodeLogic(
        IHttpContextAccessor context,
        IGraphNodeDatastore datastore,
        IGraphNodeValidator validator,
        IGraphNodeFilter filter) :
        base(context, datastore, validator, filter)
    {
    }
  }
}