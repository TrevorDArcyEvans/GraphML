using System;
using System.Collections.Generic;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class EdgeDatastore : RepositoryItemDatastore<Edge>, IEdgeDatastore
  {
    public EdgeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<EdgeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public PagedDataEx<Edge> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm)
    {
      // TODO   searchTerm
      return GetInternal(() =>
      {
        var where = $"where {nameof(Edge.SourceId)} in ({GetListIds(ids)}) or {nameof(Edge.TargetId)} in ({GetListIds(ids)})";
        var sql =
@$"select 
  * from {GetTableName()},
  (select count(*) as {nameof(PagedDataEx<Edge>.TotalCount)} from {GetTableName()} {where} )
{where}
order by {nameof(Edge.Name)} 
{AppendForFetch(pageIndex, pageSize)}";

        var retval = new PagedDataEx<Edge>();
        var items = _dbConnection.Query<Edge, long, Edge>(sql,
          (item, num) =>
          {
            retval.TotalCount = num;
            retval.Items.Add(item);
            return item;
          },
          splitOn: $"{nameof(PagedDataEx<Edge>.TotalCount)}");

        return retval;
      });
    }
  }
}
