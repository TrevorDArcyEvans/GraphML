using System;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class OrganisationDatastore : ItemDatastore<Organisation>, IOrganisationDatastore
  {
    public OrganisationDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<OrganisationDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public PagedDataEx<Organisation> GetAll(int pageIndex, int pageSize, string searchTerm)
    {
      if (pageIndex < 0)
      {
        throw new ArgumentOutOfRangeException($"{nameof(pageIndex)} starts at 0");
      }

      var where = $"where {nameof(Organisation.Name)} like '%{searchTerm ?? string.Empty}%'";
      var sql =
        @$"select
  * from {GetTableName()},
  (select count(*) as {nameof(PagedDataEx<Organisation>.TotalCount)} from {GetTableName()} {where} )
{where}
order by {nameof(Organisation.Name)}
{AppendForFetch(pageIndex, pageSize)}";

      var retval = new PagedDataEx<Organisation>();
      var items = _dbConnection.Query<Organisation, long, Organisation>(sql,
        (item, num) =>
        {
          retval.TotalCount = num;
          retval.Items.Add(item);
          return item;
        },
        splitOn: $"{nameof(PagedDataEx<Organisation>.TotalCount)}");

      return retval;
    }
  }
}
