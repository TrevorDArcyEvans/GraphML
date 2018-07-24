using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class EdgeLogic : LogicBase<Edge>, IEdgeLogic
  {
    public EdgeLogic(
      IHttpContextAccessor context,
      IEdgeDatastore datastore,
      IEdgeValidator validator,
      IEdgeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
