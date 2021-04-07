using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class RepositoryItemAttributeDefinitionDatastore : OwnedItemDatastoreBase<RepositoryItemAttributeDefinition>, IRepositoryItemAttributeDefinitionDatastore
  {
    public RepositoryItemAttributeDefinitionDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<RepositoryItemAttributeDefinitionDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
