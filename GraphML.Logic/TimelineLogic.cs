using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class TimelineLogic : OwnedLogicBase<Timeline>, ITimelineLogic
  {
    public TimelineLogic(
      IHttpContextAccessor context,
      ILogger<TimelineLogic> logger,
      ITimelineDatastore datastore,
      ITimelineValidator validator,
      ITimelineFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
