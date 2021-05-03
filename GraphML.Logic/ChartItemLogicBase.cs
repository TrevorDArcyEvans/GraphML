using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public abstract class ChartItemLogicBase<T> : OwnedLogicBase<T>, IChartItemLogic<T> where T : ChartItem, new()
  {
    private readonly IChartItemDatastore<T> _chartItemDatastore;
    
    protected ChartItemLogicBase(
      IHttpContextAccessor context, 
      IChartItemDatastore<T> datastore, 
      IValidator<T> validator, 
      IFilter<T> filter) : 
      base(context, datastore, validator, filter)
    {
      _chartItemDatastore = datastore;
    }

    public IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      var valRes = _validator.Validate(new T(), options => options.IncludeRuleSets(nameof(ByGraphItems)));
      if (valRes.IsValid)
      {
        var items = _chartItemDatastore.ByGraphItems(chartId, graphItemIds);
        var filtered = _filter.Filter(items);
        return filtered;
      }

      return Enumerable.Empty<T>();
    }
  }
}
