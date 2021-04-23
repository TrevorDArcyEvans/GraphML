using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class ChartDatastore : OwnedItemDatastoreBase<Chart>, IChartDatastore
  {
    public ChartDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
