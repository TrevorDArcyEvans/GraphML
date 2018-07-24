using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class ItemAttributeDatastore : DatastoreBase<ItemAttribute>, IItemAttributeDatastore
  {
    public ItemAttributeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ItemAttributeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
