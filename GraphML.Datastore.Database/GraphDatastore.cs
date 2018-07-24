using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphDatastore : DatastoreBase<Graph>, IGraphDatastore
  {
    public GraphDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
