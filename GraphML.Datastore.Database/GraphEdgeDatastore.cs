using System;
using System.Collections.Generic;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphEdgeDatastore : OwnedItemDatastore<GraphEdge>, IGraphEdgeDatastore
  {
    public GraphEdgeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphEdgeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public PagedDataEx<GraphEdge> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm)
    {
      return GetInternal(() =>
      {
        var where = $"where ( {nameof(GraphEdge.GraphSourceId)} in ({GetListIds(ids)}) or {nameof(GraphEdge.GraphTargetId)} in ({GetListIds(ids)}) ) and {nameof(GraphEdge.Name)} like '%{searchTerm ?? string.Empty}%'";
        var sql =
@$"select 
  * from {GetTableName()},
  (select count(*) as {nameof(PagedDataEx<GraphEdge>.TotalCount)} from {GetTableName()} {where} )
{where}
order by {nameof(GraphEdge.Name)} 
{AppendForFetch(pageIndex, pageSize)}";

        var retval = new PagedDataEx<GraphEdge>();
        var items = _dbConnection.Query<GraphEdge, long, GraphEdge>(sql,
          (item, num) =>
          {
            retval.TotalCount = num;
            retval.Items.Add(item);
            return item;
          },
          splitOn: $"{nameof(PagedDataEx<GraphEdge>.TotalCount)}");

        return retval;
      });
    }
  }
}
