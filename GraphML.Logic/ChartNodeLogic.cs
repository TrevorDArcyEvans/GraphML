using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ChartNodeLogic : OwnedLogicBase<ChartNode>, IChartNodeLogic
  {
    private readonly IChartNodeDatastore _chartItemDatastore;

    public ChartNodeLogic(
      IHttpContextAccessor context,
      IChartNodeDatastore datastore,
      IChartNodeValidator validator,
      IChartNodeFilter filter) :
      base(context, datastore, validator, filter)
    {
      _chartItemDatastore = datastore;
    }

    public IEnumerable<ChartNode> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      var valRes = _validator.Validate(new ChartNode(), options => options.IncludeRuleSets(nameof(ByGraphItems)));
      if (valRes.IsValid)
      {
        var items = _chartItemDatastore.ByGraphItems(chartId, graphItemIds);
        var filtered = _filter.Filter(items);
        return filtered;
      }

      return Enumerable.Empty<ChartNode>();
    }
  }
}
