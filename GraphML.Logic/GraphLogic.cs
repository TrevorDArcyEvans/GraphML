using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class GraphLogic : LogicBase<Graph>, IGraphLogic
  {
    public GraphLogic(
      IHttpContextAccessor context,
      IGraphDatastore datastore,
      IGraphValidator validator,
      IGraphFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
