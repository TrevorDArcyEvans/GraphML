using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
    public sealed class RoleDatastore : DatastoreBase<Role>, IRoleDatastore
    {
        public RoleDatastore(
            IDbConnectionFactory dbConnectionFactory,
            ILogger<IDatastore<Role>> logger,
            ISyncPolicyFactory policy) :
            base(dbConnectionFactory, logger, policy)
        {
        }

        public IEnumerable<Role> ByContactId(string id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Role> GetAll()
        {
            return _dbConnection.GetAll<Role>();
        }
    }
}