using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GraphML.Logic
{
  public abstract class LogicBase<T> : ILogic<T>
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

    public IQueryable<T> ByOwner(string ownerId)
    {
      return _datastore.ByOwner(ownerId);
    }

    public T Create(T entity)
    {
      return _datastore.Create(entity);
    }

    public void Delete(T entity)
    {
      _datastore.Delete(entity);
    }

    public void Update(T entity)
    {
      _datastore.Update(entity);
    }
  }
}
