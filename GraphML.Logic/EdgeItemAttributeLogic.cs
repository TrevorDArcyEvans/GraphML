using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class EdgeItemAttributeLogic : OwnedLogicBase<EdgeItemAttribute>, IEdgeItemAttributeLogic
  {
    public EdgeItemAttributeLogic(
      IHttpContextAccessor context,
      ILogger<EdgeItemAttributeLogic> logger,
      IEdgeItemAttributeDatastore datastore,
      IEdgeItemAttributeValidator validator,
      IEdgeItemAttributeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
