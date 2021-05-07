using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public abstract class GraphItemLogicBase<T> : OwnedLogicBase<T>, IGraphItemLogic<T> where T : GraphItem, new()
  {
    private readonly IGraphItemDatastore<T> _graphItemDatastore;
    
    protected GraphItemLogicBase(
      IHttpContextAccessor context, 
      IGraphItemDatastore<T> datastore, 
      IValidator<T> validator, 
      IFilter<T> filter) : 
      base(context, datastore, validator, filter)
    {
      _graphItemDatastore = datastore;
    }

    public IEnumerable<T> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds)
    {
      var valRes = _validator.Validate(new T(), options => options.IncludeRuleSets(nameof(ByRepositoryItems)));
      if (valRes.IsValid)
      {
        var items = _graphItemDatastore.ByRepositoryItems(graphId, repoItemIds);
        var filtered = _filter.Filter(items);
        return filtered;
      }

      return Enumerable.Empty<T>();
    }
  }
}
