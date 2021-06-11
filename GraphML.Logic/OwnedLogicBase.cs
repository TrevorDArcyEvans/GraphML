using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public abstract class OwnedLogicBase<T> : LogicBase<T>, IOwnedLogic<T> where T : OwnedItem, new()
  {
    private readonly IOwnedDatastore<T> _ownedDatastore;

    public OwnedLogicBase(
      IHttpContextAccessor context,
      ILogger<OwnedLogicBase<T>> logger,
      IOwnedDatastore<T> datastore,
      IValidator<T> validator,
      IFilter<T> filter) :
      base(context, logger, datastore, validator, filter)
    {
      _ownedDatastore = datastore;
    }

    public virtual PagedDataEx<T> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex, int pageSize, string searchTerm)
    {
      var valRes = _validator.Validate(new T(), options => options.IncludeRuleSets(nameof(IOwnedLogic<T>.ByOwners)));
      if (valRes.IsValid)
      {
        var pdex = _ownedDatastore.ByOwners(ownerIds, pageIndex, pageSize, searchTerm);
        var filtered = _filter.Filter(pdex.Items);
        return new PagedDataEx<T>
        {
          TotalCount = pdex.TotalCount,
          Items = filtered.ToList()
        };
      }

      _logger.LogError(valRes.ToString());
      return new PagedDataEx<T>();
    }

    public int Count(Guid ownerId)
    {
      var valRes = _validator.Validate(new T(), options => options.IncludeRuleSets(nameof(IOwnedLogic<T>.Count)));
      if (!valRes.IsValid)
      {
        return 0;
      }

      _logger.LogError(valRes.ToString());
      return _ownedDatastore.Count(ownerId);
    }
  }
}
