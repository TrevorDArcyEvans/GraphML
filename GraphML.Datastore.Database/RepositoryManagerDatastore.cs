using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class RepositoryManagerDatastore : OwnedItemDatastoreBase<RepositoryManager>, IRepositoryManagerDatastore
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
        return _dbConnection.GetAll<RepositoryManager>().Where(rm => ids.Contains(rm.Id));
      });
    }

    public IEnumerable<RepositoryManager> GetAll()
    {
      return GetInternal(() =>
      {
        return _dbConnection.GetAll<RepositoryManager>();
      });
    }
  }
}
