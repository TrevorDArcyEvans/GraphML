using System;
using System.Collections.Generic;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using GraphML.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database.Porcelain
{
  public sealed class ChartEdgeExDatastore : DatastoreBase, IChartEdgeExDatastore
  {
    public ChartEdgeExDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartEdgeExDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<ChartEdgeEx> ByOwner(Guid chartId)
    {
      var chartEdgeExSql =
        $@"select
 ci.Id,
 ci.OrganisationId,
 ci.Name,
 ci.OwnerId,
 ci.GraphItemId,
 ri.Id as RepositoryItemId,
 ri.SourceId,
 ri.TargetId
from ChartEdge ci
join GraphEdge gn on gn.Id = ci.GraphItemId
join Edge ri on ri.Id = gn.RepositoryItemId
where ci.OwnerId='{chartId}'";
      var chartEdgeEx = _dbConnection.Query<ChartEdgeEx>(chartEdgeExSql);

      return chartEdgeEx;
    }
  }
}
