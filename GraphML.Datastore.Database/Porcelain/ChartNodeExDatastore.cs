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
  public sealed class ChartNodeExDatastore : DatastoreBase, IChartNodeExDatastore
  {
    public ChartNodeExDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartNodeExDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public IEnumerable<ChartNodeEx> ByOwner(Guid chartId)
    {
      var chartNodeExSql =
        $@"select
 ci.Id,
 ci.OrganisationId,
 ci.Name,
 ci.GraphItemId,
 ri.Id as RepositoryItemId,
 ci.X,
 ci.Y
from ChartNode ci
join GraphNode gn on gn.Id = ci.GraphItemId
join Node ri on ri.Id = gn.RepositoryItemId
where ci.OwnerId='{chartId}'";
      var chartNodeEx = _dbConnection.Query<ChartNodeEx>(chartNodeExSql);

      return chartNodeEx;
    }
  }
}
