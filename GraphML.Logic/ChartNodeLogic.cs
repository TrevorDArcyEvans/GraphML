using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ChartNodeLogic : OwnedLogicBase<ChartNode>, IChartNodeLogic
  {
    public ChartNodeLogic(
      IHttpContextAccessor context,
      IChartNodeDatastore datastore,
      IChartNodeValidator validator,
      IChartNodeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
