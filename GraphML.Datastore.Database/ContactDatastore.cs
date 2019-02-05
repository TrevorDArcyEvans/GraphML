using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class ContactDatastore : OwnedItemDatastoreBase<Contact>, IContactDatastore
  {
    public ContactDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ContactDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
