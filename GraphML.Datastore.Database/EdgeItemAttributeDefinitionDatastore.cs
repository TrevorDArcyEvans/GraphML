using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class EdgeItemAttributeDefinitionDatastore : OwnedItemDatastore<EdgeItemAttributeDefinition>, IEdgeItemAttributeDefinitionDatastore
  {
    public EdgeItemAttributeDefinitionDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<EdgeItemAttributeDefinitionDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
