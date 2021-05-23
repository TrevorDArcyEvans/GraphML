using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class TimelineNodeLogic : TimelineItemLogicBase<TimelineNode>, ITimelineNodeLogic
  {
    public TimelineNodeLogic(
      IHttpContextAccessor context,
      ITimelineNodeDatastore datastore,
      ITimelineNodeValidator validator,
      ITimelineNodeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
