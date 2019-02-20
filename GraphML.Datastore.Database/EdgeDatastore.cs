using System.Collections.Generic;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class EdgeDatastore : OwnedItemDatastoreBase<Edge>, IEdgeDatastore
  {
    public EdgeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<EdgeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<Edge> ByNodeIds(IEnumerable<string> ids, int pageIndex, int pageSize)
    {
      return GetInternal(() =>
      {
        var sql = $"select * from {GetTableName()} where {nameof(Edge.SourceId)} in ({GetListIds(ids)}) or {nameof(Edge.TargetId)} in ({GetListIds(ids)}) order by {nameof(OwnedItem.Id)} {AppendForFetch(pageIndex, pageSize)}";

        return _dbConnection.Query<Edge>(sql);
      });
    }
  }
}
