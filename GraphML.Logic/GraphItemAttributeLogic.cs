using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class GraphItemAttributeLogic : OwnedLogicBase<GraphItemAttribute>, IGraphItemAttributeLogic
  {
    public GraphItemAttributeLogic(
      IHttpContextAccessor context,
      ILogger<GraphItemAttributeLogic> logger,
      IGraphItemAttributeDatastore datastore,
      IGraphItemAttributeValidator validator,
      IGraphItemAttributeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
