using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class TimelineEdgeLogic : TimelineItemLogicBase<TimelineEdge>, ITimelineEdgeLogic
  {
    public TimelineEdgeLogic(
      IHttpContextAccessor context,
      ITimelineEdgeDatastore datastore,
      ITimelineEdgeValidator validator,
      ITimelineEdgeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
