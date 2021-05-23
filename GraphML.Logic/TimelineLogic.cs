using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class TimelineLogic : OwnedLogicBase<Timeline>, ITimelineLogic
  {
    public TimelineLogic(
      IHttpContextAccessor context,
      ITimelineDatastore datastore,
      ITimelineValidator validator,
      ITimelineFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
