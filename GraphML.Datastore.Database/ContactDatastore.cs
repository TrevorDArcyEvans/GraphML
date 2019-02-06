using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;

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

    public Contact ByEmail(string email)
    {
      return _dbConnection.GetAll<Contact>().SingleOrDefault(c => c.Name.ToLowerInvariant() == email.ToLowerInvariant());
    }
  }
}
