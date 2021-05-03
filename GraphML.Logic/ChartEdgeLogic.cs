using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ChartEdgeLogic : OwnedLogicBase<ChartEdge>, IChartEdgeLogic
  {
    private readonly IChartEdgeDatastore _chartItemDatastore;

    public ChartEdgeLogic(
      IHttpContextAccessor context,
      IChartEdgeDatastore datastore,
      IChartEdgeValidator validator,
      IChartEdgeFilter filter) :
      base(context, datastore, validator, filter)
    {
      _chartItemDatastore = datastore;
    }

    public IEnumerable<ChartEdge> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      var valRes = _validator.Validate(new ChartEdge(), options => options.IncludeRuleSets(nameof(ByGraphItems)));
      if (valRes.IsValid)
      {
        var items = _chartItemDatastore.ByGraphItems(chartId, graphItemIds);
        var filtered = _filter.Filter(items);
        return filtered;
      }

      return Enumerable.Empty<ChartEdge>();
    }
  }
}
