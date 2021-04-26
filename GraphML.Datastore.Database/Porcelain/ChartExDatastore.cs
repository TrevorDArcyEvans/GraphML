using System;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using GraphML.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database.Porcelain
{
  public sealed class ChartExDatastore : DatastoreBase,  IChartExDatastore
  {
    private readonly IChartNodeExDatastore _chartNodeExDatastore;
    private readonly IChartEdgeExDatastore _chartEdgeExDatastore;

    public ChartExDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartExDatastore> logger,
      ISyncPolicyFactory policy,
      IChartNodeExDatastore chartNodeExDatastore,
      IChartEdgeExDatastore chartEdgeExDatastore) :
      base(dbConnectionFactory, logger, policy)
    {
      _chartNodeExDatastore = chartNodeExDatastore;
      _chartEdgeExDatastore = chartEdgeExDatastore;
    }

    public ChartEx ById(Guid chartId)
    {
      var chartExSql =
        $@"select
 c.*
from Chart c
where c.Id='{chartId}'";
      var chartEx = _dbConnection.QueryFirst<ChartEx>(chartExSql);

      chartEx.Nodes = _chartNodeExDatastore.ByOwner(chartId);
      chartEx.Edges = _chartEdgeExDatastore.ByOwner(chartId);

      return chartEx;
    }
  }
}
