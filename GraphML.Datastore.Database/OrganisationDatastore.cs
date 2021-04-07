using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class OrganisationDatastore : DatastoreBase<Organisation>, IOrganisationDatastore
  {
    public OrganisationDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<OrganisationDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<Organisation> GetAll()
    {
      return _dbConnection.GetAll<Organisation>();
    }
  }
}
