﻿using System;
using Dapper;
using GraphML.Datastore.Database.Interfaces;
using GraphML.Interfaces;
using GraphML.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.Extensions.Logging;

namespace GraphML.Datastore.Database.Porcelain
{
  public sealed class ChartExDatastore : OwnedItemDatastoreBase<ChartEx>, IChartExDatastore
  {
    public ChartExDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartExDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public ChartEx ById(Guid id)
    {
      var chartExSql = 
$@"select
 c.*
from Chart c
where c.Id='{id}'";
      var chartEx = _dbConnection.QueryFirst<ChartEx>(chartExSql);

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
where ci.OwnerId='{id}'";
      var chartNodeEx = _dbConnection.Query<ChartNodeEx>(chartNodeExSql);
      
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
where ci.OwnerId='{id}'";
      var chartEdgeEx = _dbConnection.Query<ChartEdgeEx>(chartEdgeExSql);

      chartEx.Nodes = chartNodeEx;
      chartEx.Edges = chartEdgeEx;
      
      return chartEx;
    }
  }
  public sealed class ChartNodeExDatastore : OwnedItemDatastoreBase<ChartNodeEx>, IChartNodeExDatastore
  {
    public ChartNodeExDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartNodeExDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
  public sealed class ChartEdgeExDatastore : OwnedItemDatastoreBase<ChartEdgeEx>, IChartEdgeExDatastore
  {
    public ChartEdgeExDatastore(
      IDbConnectionFactory dbConnectionFactory,
      ILogger<ChartEdgeExDatastore> logger,
      ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }
  }
}