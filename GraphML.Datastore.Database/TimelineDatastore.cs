using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class TimelineDatastore : OwnedItemDatastore<Timeline>, ITimelineDatastore
  {
    public TimelineDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<TimelineDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
