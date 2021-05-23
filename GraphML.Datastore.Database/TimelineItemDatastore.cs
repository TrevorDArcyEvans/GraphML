using System;
using System.Collections.Generic;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database
{
  public abstract class TimelineItemDatastore<T> : OwnedItemDatastore<T> where T : TimelineItem, new()
  {
    protected TimelineItemDatastore(
      IDbConnectionFactory dbConnectionFactory, 
      ILogger<TimelineItemDatastore<T>> logger, 
      ISyncPolicyFactory policy) : 
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      return GetInternal(() =>
      {
        var where = $"where {nameof(TimelineItem.OwnerId)} = '{chartId}' and {nameof(TimelineItem.GraphItemId)} in ({GetListIds(graphItemIds)})";
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
