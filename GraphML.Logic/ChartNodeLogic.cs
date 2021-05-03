using System;
using System.Collections.Generic;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ChartNodeLogic : OwnedLogicBase<ChartNode>, IChartNodeLogic
  {
    private readonly IChartNodeDatastore _chartNodeDatastore;
    
    public ChartNodeLogic(
      IHttpContextAccessor context,
      IChartNodeDatastore datastore,
      IChartNodeValidator validator,
      IChartNodeFilter filter) :
      base(context, datastore, validator, filter)
    {
      _chartNodeDatastore = datastore;
    }

    public IEnumerable<ChartNode> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      // TODO   validation
      // TODO   filter
      return _chartNodeDatastore.ByGraphItems(chartId, graphItemIds);
    }
  }
}
