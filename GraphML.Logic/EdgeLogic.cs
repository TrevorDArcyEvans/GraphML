using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class EdgeLogic : OwnedLogicBase<Edge>, IEdgeLogic
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
