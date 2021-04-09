using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphNodeDatastore : OwnedItemDatastoreBase<GraphNode>, IGraphNodeDatastore
  {
    public GraphNodeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphNodeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
