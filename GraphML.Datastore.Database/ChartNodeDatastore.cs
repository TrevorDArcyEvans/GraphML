using System;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public sealed class ChartNodeDatastore : OwnedItemDatastore<ChartNode>, IChartNodeDatastore
  {
    public ChartNodeDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartNodeDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public ChartNode ByGraphItem(Guid chartId, Guid graphItemId)
    {
      return GetInternal(() =>
      {
        var where = $"where {nameof(ChartItem.OwnerId)} = '{chartId}' and {nameof(ChartItem.GraphItemId)} = '{graphItemId}'";
        var sql = 
          @$"select
  * from {GetTableName()}
{where}";

        var retval = _dbConnection.QueryFirstOrDefault<ChartNode>(sql);

        return retval;
      });
    }
  }
}
