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
  public sealed class ChartExDatastore : ItemDatastore<ChartEx>,  IChartExDatastore
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

  public sealed class ChartNodeExDatastore : ItemDatastore<ChartNodeEx>, IChartNodeExDatastore
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

  public sealed class ChartEdgeExDatastore : ItemDatastore<ChartEdgeEx>, IChartEdgeExDatastore
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
