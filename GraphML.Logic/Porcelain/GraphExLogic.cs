using GraphML.Interfaces.Porcelain;
using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Porcelain
{
  public sealed class GraphExLogic : LogicBase<GraphEx>, IGraphExLogic
  {
    public GraphExLogic(
      IHttpContextAccessor context,
      IGraphExDatastore datastore,
      IGraphExValidator validator,
      IGraphExFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}

