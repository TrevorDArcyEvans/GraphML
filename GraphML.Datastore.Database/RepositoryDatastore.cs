using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class RepositoryDatastore : DatastoreBase<Repository>, IRepositoryDatastore
  {
    public RepositoryDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<RepositoryDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
