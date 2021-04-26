using System;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public abstract class RepositoryItemDatastore<T> : OwnedItemDatastore<T>, IRepositoryItemDatastore<T> where T : RepositoryItem
  {
    protected RepositoryItemDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<RepositoryItemDatastore<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public PagedDataEx<T> GetParents(Guid itemId, int pageIndex, int pageSize, string searchTerm)
    {
      return GetInternal(() =>
      {
        var where = $"where NextId = '{itemId}' and {nameof(RepositoryItem.Name)} like '%{searchTerm ?? string.Empty}%'";
        var sql = 
@$"select
  * from {GetTableName()},
  (select count(*) as {nameof(PagedDataEx<T>.TotalCount)} from {GetTableName()} {where} )
{where}
order by {nameof(RepositoryItem.Name)}
{AppendForFetch(pageIndex, pageSize)}";

        var retval = new PagedDataEx<T>();
        var items = _dbConnection.Query<T, long, T>(sql,
          (item, num) =>
          {
            retval.TotalCount = num;
            retval.Items.Add(item);
            return item;
          },
          splitOn: $"{nameof(PagedDataEx<T>.TotalCount)}");

        return retval;
      });
    }
  }
}
