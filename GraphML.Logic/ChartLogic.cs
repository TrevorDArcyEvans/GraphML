using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class ChartLogic : OwnedLogicBase<Chart>, IChartLogic
  {
    public ChartLogic(
      IHttpContextAccessor context,
      ILogger<ChartLogic> logger,
      IChartDatastore datastore,
      IChartValidator validator,
      IChartFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
