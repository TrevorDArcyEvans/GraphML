using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GraphML.Logic
{
  public abstract class LogicBase<T> : ILogic<T> where T : class
  {
    protected readonly IHttpContextAccessor _context;
    protected readonly IDatastore<T> _datastore;
    protected readonly IValidator<T> _validator;
    protected readonly IFilter<T> _filter;

    public LogicBase(
      IHttpContextAccessor context,
      IDatastore<T> datastore,
      IValidator<T> validator,
      IFilter<T> filter)
    {
      _context = context;
      _datastore = datastore;
      _validator = validator;
      _filter = filter;
    }

    public T ById(string id)
    {
      var valRes = _validator.Validate(null, ruleSet: nameof(ILogic<T>.ByOwner));
      return _filter.Filter(valRes.IsValid ? new T[] { _datastore.ById(id) }.AsQueryable() : Enumerable.Empty<T>().AsQueryable()).SingleOrDefault();
    }

    public IQueryable<T> ByOwner(string ownerId)
    {
      var valRes = _validator.Validate(null, ruleSet: nameof(ILogic<T>.ByOwner));
      return _filter.Filter(valRes.IsValid ? _datastore.ByOwner(ownerId) : Enumerable.Empty<T>().AsQueryable());
    }

    public T Create(T entity)
    {
      var valRes = _validator.Validate(entity, ruleSet: nameof(ILogic<T>.Create));
      if (!valRes.IsValid)
      {
        return null;
      }
      return _filter.Filter(new T[] { _datastore.Create(entity) }.AsQueryable()).SingleOrDefault();
    }

    public void Delete(T entity)
    {
      var valRes = _validator.Validate(entity, ruleSet: nameof(ILogic<T>.Delete));
      if (valRes.IsValid)
      {
        _datastore.Delete(entity);
      }
    }

    public void Update(T entity)
    {
      var valRes = _validator.Validate(entity, ruleSet: nameof(ILogic<T>.Update));
      if (valRes.IsValid)
      {
        _datastore.Update(entity);
      }
    }
  }
}
