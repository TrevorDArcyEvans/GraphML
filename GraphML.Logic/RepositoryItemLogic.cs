using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public abstract class RepositoryItemLogic<T> : OwnedLogicBase<T>, IRepositoryItemLogic<T> where T : RepositoryItem, new()
  {
    private readonly IRepositoryItemDatastore<T> _repositoryItemDatastore;

    public RepositoryItemLogic(
      IHttpContextAccessor context,
      IRepositoryItemDatastore<T> datastore,
      IValidator<T> validator,
      IFilter<T> filter) :
      base(context, datastore, validator, filter)
    {
      _repositoryItemDatastore = datastore;
    }

    public IEnumerable<T> GetParents(Guid itemId, int pageIndex, int pageSize)
    {
      var valRes = _validator.Validate(new T(), options => options.IncludeRuleSets(nameof(IRepositoryItemLogic<T>.GetParents)));
      if (valRes.IsValid)
      {
        return _filter.Filter(_repositoryItemDatastore.GetParents(itemId, pageIndex, pageSize));
      }

      return Enumerable.Empty<T>();
    }
  }
}
