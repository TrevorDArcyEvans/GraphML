using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ChartEdgeLogic : ChartItemLogicBase<ChartEdge>, IChartEdgeLogic
  {
    public ChartEdgeLogic(
      IHttpContextAccessor context,
      IChartEdgeDatastore datastore,
      IChartEdgeValidator validator,
      IChartEdgeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
