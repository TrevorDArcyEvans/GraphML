using System;
using System.Collections.Generic;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ChartEdgeLogic : OwnedLogicBase<ChartEdge>, IChartEdgeLogic
  {
    private readonly IChartEdgeDatastore _chartEdgeDatastore;

    public ChartEdgeLogic(
      IHttpContextAccessor context,
      IChartEdgeDatastore datastore,
      IChartEdgeValidator validator,
      IChartEdgeFilter filter) :
      base(context, datastore, validator, filter)
    {
      _chartEdgeDatastore = datastore;
    }

    public IEnumerable<ChartEdge> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      // TODO   validation
      // TODO   filter
      return _chartEdgeDatastore.ByGraphItems(chartId, graphItemIds);
    }
  }
}
