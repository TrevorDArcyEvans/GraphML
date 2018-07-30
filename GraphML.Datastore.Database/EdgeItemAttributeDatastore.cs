using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class EdgeItemAttributeDatastore : OwnedItemDatastoreBase<EdgeItemAttribute>, IEdgeItemAttributeDatastore
  {
    public EdgeItemAttributeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<EdgeItemAttributeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
