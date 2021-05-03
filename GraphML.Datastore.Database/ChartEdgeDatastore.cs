using System;
using System.Collections.Generic;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class ChartEdgeDatastore : OwnedItemDatastore<ChartEdge>, IChartEdgeDatastore
  {
    public ChartEdgeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartEdgeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<ChartEdge> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      return GetInternal(() =>
      {
        var where = $"where {nameof(ChartItem.OwnerId)} = '{chartId}' and {nameof(ChartItem.GraphItemId)} in ({GetListIds(graphItemIds)})";
        var sql = 
          @$"select
  * from {GetTableName()}
{where}";

        var retval = _dbConnection.Query<ChartEdge>(sql);

        return retval;
      });
    }
  }
}
