using System;
using System.Collections.Generic;
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

    public IEnumerable<ChartNode> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      return GetInternal(() =>
      {
        var where = $"where {nameof(ChartItem.OwnerId)} = '{chartId}' and {nameof(ChartItem.GraphItemId)} in ({GetListIds(graphItemIds)})";
        var sql = 
          @$"select
  * from {GetTableName()}
{where}";

        var retval = _dbConnection.Query<ChartNode>(sql);

        return retval;
      });
    }
  }
}
