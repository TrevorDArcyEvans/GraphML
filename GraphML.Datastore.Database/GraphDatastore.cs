using System;
using System.Collections.Generic;
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

    public IEnumerable<Graph> ByEdgeId(Guid id, int pageIndex, int pageSize)
    {
      return GetInternal(() =>
      {
        var sql = $"select g.* from {GetTableName()} g join GraphEdge gi on g.Id = gi.OwnerId where gi.RepositoryItemId = '{id}' {AppendForFetch(pageIndex, pageSize)}";

        return _dbConnection.Query<Graph>(sql);
      });
    }

    public IEnumerable<Graph> ByNodeId(Guid id, int pageIndex, int pageSize)
    {
      return GetInternal(() =>
      {
        var sql = $"select g.* from {GetTableName()} g join GraphNode gi on g.Id = gi.OwnerId where gi.RepositoryItemId = '{id}' {AppendForFetch(pageIndex, pageSize)}";

        return _dbConnection.Query<Graph>(sql);
      });
    }
  }
}
