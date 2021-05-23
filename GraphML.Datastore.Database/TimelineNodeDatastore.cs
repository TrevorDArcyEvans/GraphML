using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class TimelineNodeDatastore : TimelineItemDatastore<TimelineNode>, ITimelineNodeDatastore
  {
    public TimelineNodeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<TimelineNodeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
