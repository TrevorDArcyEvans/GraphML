using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class GraphItemAttributeLogic : OwnedLogicBase<GraphItemAttribute>, IGraphItemAttributeLogic
  {
    public GraphItemAttributeLogic(
      IHttpContextAccessor context,
      IGraphItemAttributeDatastore datastore,
      IGraphItemAttributeValidator validator,
      IGraphItemAttributeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
