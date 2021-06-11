using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class TimelineNodeLogic : TimelineItemLogicBase<TimelineNode>, ITimelineNodeLogic
  {
    public TimelineNodeLogic(
      IHttpContextAccessor context,
      ILogger<TimelineNodeLogic> logger,
      ITimelineNodeDatastore datastore,
      ITimelineNodeValidator validator,
      ITimelineNodeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
