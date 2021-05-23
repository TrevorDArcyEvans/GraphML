using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public abstract class TimelineItemLogicBase<T> : OwnedLogicBase<T>, ITimelineItemLogic<T> where T : TimelineItem, new()
  {
    private readonly ITimelineItemDatastore<T> _timelineItemDatastore;
    
    protected TimelineItemLogicBase(
      IHttpContextAccessor context, 
      ITimelineItemDatastore<T> datastore, 
      IValidator<T> validator, 
      IFilter<T> filter) : 
      base(context, datastore, validator, filter)
    {
      _timelineItemDatastore = datastore;
    }

    public IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds)
    {
      var valRes = _validator.Validate(new T(), options => options.IncludeRuleSets(nameof(ByGraphItems)));
      if (valRes.IsValid)
      {
        var items = _timelineItemDatastore.ByGraphItems(chartId, graphItemIds);
        var filtered = _filter.Filter(items);
        return filtered;
      }

      return Enumerable.Empty<T>();
    }
  }
}
