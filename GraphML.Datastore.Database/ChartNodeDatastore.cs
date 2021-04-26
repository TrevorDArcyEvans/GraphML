using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class ChartNodeDatastore : OwnedItemDatastore<ChartNode>, IChartNodeDatastore
  {
    public ChartNodeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartNodeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
