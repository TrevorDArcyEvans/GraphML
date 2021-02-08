using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class GraphEdgeLogic : OwnedLogicBase<GraphEdge>, IGraphEdgeLogic
  {
    public GraphEdgeLogic(
        IHttpContextAccessor context,
        IGraphEdgeDatastore datastore,
        IGraphEdgeValidator validator,
        IGraphEdgeFilter filter) :
        base(context, datastore, validator, filter)
    {
    }
  }
}