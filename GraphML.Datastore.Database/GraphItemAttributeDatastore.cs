using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphItemAttributeDatastore : OwnedItemDatastore<GraphItemAttribute>, IGraphItemAttributeDatastore
  {
    public GraphItemAttributeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphItemAttributeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
