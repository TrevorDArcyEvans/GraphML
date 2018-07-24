using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class EdgeDatastore : DatastoreBase<Edge>, IEdgeDatastore
  {
    public EdgeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<EdgeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
