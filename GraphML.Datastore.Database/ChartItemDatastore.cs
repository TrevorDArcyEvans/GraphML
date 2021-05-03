using System;
using System.Collections.Generic;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public abstract class ChartItemDatastore<T> : OwnedItemDatastore<T> where T : ChartItem, new()
  {
    protected ChartItemDatastore(
      IDbConnectionFactory dbConnectionFactory, 
      ILogger<ChartItemDatastore<T>> logger, 
      ISyncPolicyFactory policy) : 
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      return GetInternal(() =>
      {
        var where = $"where {nameof(ChartItem.OwnerId)} = '{chartId}' and {nameof(ChartItem.GraphItemId)} in ({GetListIds(graphItemIds)})";
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
