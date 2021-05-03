using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class ChartEdgeDatastore : ChartItemDatastore<ChartEdge>, IChartEdgeDatastore
  {
    public ChartEdgeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartEdgeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
