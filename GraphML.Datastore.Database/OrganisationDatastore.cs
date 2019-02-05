using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class OrganisationDatastore : DatastoreBase<Organisation>, IOrganisationDatastore
  {
    public OrganisationDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<IDatastore<Organisation>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}
