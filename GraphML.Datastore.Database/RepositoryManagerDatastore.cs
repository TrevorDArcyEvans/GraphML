using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class RepositoryManagerDatastore : DatastoreBase<RepositoryManager>, IRepositoryManagerDatastore
  {
    public RepositoryManagerDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<RepositoryManagerDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public override IEnumerable<RepositoryManager> ByIds(IEnumerable<string> ids)
    {
      return GetInternal(() =>
      {
        return _dbConnection.GetAll<RepositoryManager>();
      });
    }
  }
}
