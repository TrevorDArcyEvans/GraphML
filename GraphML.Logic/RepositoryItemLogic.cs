using System;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public abstract class RepositoryItemLogic<T> : OwnedLogicBase<T>, IRepositoryItemLogic<T> where T : RepositoryItem, new()
  {
    private readonly IRepositoryItemDatastore<T> _repositoryItemDatastore;

    public RepositoryItemLogic(
      IHttpContextAccessor context,
      ILogger<RepositoryItemLogic<T>> logger,
      IRepositoryItemDatastore<T> datastore,
      IValidator<T> validator,
      IFilter<T> filter) :
      base(context, logger, datastore, validator, filter)
    {
      _repositoryItemDatastore = datastore;
    }

    public PagedDataEx<T> GetParents(Guid itemId, int pageIndex, int pageSize, string searchTerm)
    {
      var valRes = _validator.Validate(new T(), options => options.IncludeRuleSets(nameof(IRepositoryItemLogic<T>.GetParents)));
      if (valRes.IsValid)
      {
        var pdex = _repositoryItemDatastore.GetParents(itemId, pageIndex, pageSize, searchTerm);
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
  }
}
