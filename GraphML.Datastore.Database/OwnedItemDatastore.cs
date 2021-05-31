using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace GraphML.Datastore.Database
{
  public abstract class OwnedItemDatastore<T> : ItemDatastore<T>, IOwnedDatastore<T> where T : OwnedItem
  {
    public OwnedItemDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<OwnedItemDatastore<T>> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public virtual PagedDataEx<T> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex, int pageSize, string searchTerm)
    {
      if (pageIndex < 0)
      {
        throw new ArgumentOutOfRangeException($"{nameof(pageIndex)} starts at 0");
      }
      
      return GetInternal(() =>
      {
        var where = $"where {nameof(OwnedItem.OwnerId)} in ({GetListIds(ownerIds)}) and {nameof(OwnedItem.Name)} like '%{searchTerm ?? string.Empty}%'";
        var sql = 
@$"select
  * from {GetTableName()},
  (select count(*) as {nameof(PagedDataEx<T>.TotalCount)} from {GetTableName()} {where} )
{where}
order by {nameof(OwnedItem.Name)}
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

    public int Count(Guid ownerId)
    {
      return GetInternal(() =>
      {
        var sql = $"select count(*) from {GetTableName()} where {nameof(OwnedItem.OwnerId)} = '{ownerId}'";

        return _dbConnection.QueryFirst<int>(sql);
      });
    }
  }
}
