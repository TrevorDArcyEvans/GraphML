using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ChartLogic : OwnedLogicBase<Chart>, IChartLogic
  {
    public ChartLogic(
      IHttpContextAccessor context,
      IChartDatastore datastore,
      IChartValidator validator,
      IChartFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
