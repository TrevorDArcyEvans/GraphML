using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class ChartEdgeLogic : ChartItemLogicBase<ChartEdge>, IChartEdgeLogic
  {
    public ChartEdgeLogic(
      IHttpContextAccessor context,
      ILogger<ChartEdgeLogic> logger,
      IChartEdgeDatastore datastore,
      IChartEdgeValidator validator,
      IChartEdgeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
