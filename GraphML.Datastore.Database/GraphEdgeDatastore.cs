using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphEdgeDatastore : OwnedItemDatastore<GraphEdge>, IGraphEdgeDatastore
  {
    public GraphEdgeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphEdgeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
