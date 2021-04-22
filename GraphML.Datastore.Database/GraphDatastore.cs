using System;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class GraphDatastore : OwnedItemDatastoreBase<Graph>, IGraphDatastore
  {
    public GraphDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<GraphDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public PagedDataEx<Graph> ByEdgeId(Guid id, int pageIndex, int pageSize, string searchTerm)
    {
      return GetInternal(() =>
      {
        var where = $"where gi.RepositoryItemId = '{id}' and g.{nameof(Graph.Name)} like '%{searchTerm ?? string.Empty}%'";
        var join = $"join GraphEdge gi on g.Id = gi.OwnerId";
        var sql = @$"
select
  g.*,
  count(distinct g.Id) as {nameof(PagedDataEx<Graph>.TotalCount)}
from {GetTableName()} g
{join}
{where}
{AppendForFetch(pageIndex, pageSize)}";

        var retval = new PagedDataEx<Graph>();
        var items = _dbConnection.Query<Graph, long, Graph>(sql,
          (item, num) =>
          {
            retval.TotalCount = num;
            retval.Items.Add(item);
            return item;
          },
          splitOn: $"{nameof(PagedDataEx<Graph>.TotalCount)}");

        return retval;
      });
    }

    public PagedDataEx<Graph> ByNodeId(Guid id, int pageIndex, int pageSize, string searchTerm)
    {
      return GetInternal(() =>
      {
        var where = $"where gi.RepositoryItemId = '{id}' and g.{nameof(Graph.Name)} like '%{searchTerm ?? string.Empty}%'";
        var join = $"join GraphNode gi on g.Id = gi.OwnerId";
        var sql = @$"
select
  g.*,
  count(distinct g.Id) as {nameof(PagedDataEx<Graph>.TotalCount)}
from {GetTableName()} g
{join}
{where}
{AppendForFetch(pageIndex, pageSize)}";

        var retval = new PagedDataEx<Graph>();
        var items = _dbConnection.Query<Graph, long, Graph>(sql,
          (item, num) =>
          {
            retval.TotalCount = num;
            retval.Items.Add(item);
            return item;
          },
          splitOn: $"{nameof(PagedDataEx<Graph>.TotalCount)}");

        return retval;
      });
    }
  }
}
