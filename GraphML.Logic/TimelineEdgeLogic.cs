using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class TimelineEdgeLogic : TimelineItemLogicBase<TimelineEdge>, ITimelineEdgeLogic
  {
    public TimelineEdgeLogic(
      IHttpContextAccessor context,
      ILogger<TimelineEdgeLogic> logger,
      ITimelineEdgeDatastore datastore,
      ITimelineEdgeValidator validator,
      ITimelineEdgeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
    }
  }
}
