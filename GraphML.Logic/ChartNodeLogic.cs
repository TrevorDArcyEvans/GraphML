using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class ChartNodeLogic : ChartItemLogicBase<ChartNode>, IChartNodeLogic
  {
    public ChartNodeLogic(
      IHttpContextAccessor context,
      ILogger<ChartNodeLogic> logger,
      IChartNodeDatastore datastore,
      IChartNodeValidator validator,
      IChartNodeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
