using System;
using System.Collections.Generic;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public abstract class GraphItemDatastore<T> : OwnedItemDatastore<T> where T : GraphItem, new()
  {
    protected GraphItemDatastore(
      IDbConnectionFactory dbConnectionFactory, 
      ILogger<GraphItemDatastore<T>> logger, 
      ISyncPolicyFactory policy) : 
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<T> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds)
    {
      return GetInternal(() =>
      {
        var where = $"where {nameof(GraphItem.OwnerId)} = '{graphId}' and {nameof(GraphItem.RepositoryItemId)} in ({GetListIds(repoItemIds)})";
        var sql = 
          @$"select
  * from {GetTableName()}
{where}";

        var retval = _dbConnection.Query<T>(sql);

        return retval;
      });
    }
  }
}
