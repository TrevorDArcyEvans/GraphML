using System;
using System.Collections.Generic;
using Dapper;
using Dapper.Contrib.Extensions;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class RoleDatastore : ItemDatastore<Role>, IRoleDatastore
  {
    public RoleDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<RoleDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<Role> ByContactId(Guid id)
    {
      const string sql = @"
SELECT r.* FROM Role r
JOIN ContactsRoles cr on cr.RoleId = r.Id
WHERE cr.ContactId = @id
";
      return _dbConnection.Query<Role>(sql, new {id});
    }

    public IEnumerable<Role> GetAll()
    {
      return _dbConnection.GetAll<Role>();
    }
  }
}
