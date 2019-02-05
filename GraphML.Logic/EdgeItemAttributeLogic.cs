using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class EdgeItemAttributeLogic : OwnedLogicBase<EdgeItemAttribute>, IEdgeItemAttributeLogic
  {
    public EdgeItemAttributeLogic(
      IHttpContextAccessor context,
      IEdgeItemAttributeDatastore datastore,
      IEdgeItemAttributeValidator validator,
      IEdgeItemAttributeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
