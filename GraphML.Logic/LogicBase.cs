using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace GraphML.Logic
{
  public abstract class LogicBase<T> : ILogic<T> where T : Item, new()
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

    public virtual IEnumerable<T> Ids(IEnumerable<string> ids)
    {
      var valRes = _validator.Validate(new T(), ruleSet: nameof(ILogic<T>.Ids));
      if (valRes.IsValid)
      {
        return _filter.Filter(_datastore.ByIds(ids));
      }

      return null;
    }

    public virtual IEnumerable<T> Create(IEnumerable<T> entity)
    {
      foreach (var ent in entity)
      {
        var valRes = _validator.Validate(ent, ruleSet: nameof(ILogic<T>.Create));
        if (!valRes.IsValid)
        {
          return null;
        }
      }

      return _filter.Filter(_datastore.Create(entity));
    }

    public virtual void Delete(IEnumerable<T> entity)
    {
      foreach (var ent in entity)
      {
        var valRes = _validator.Validate(ent, ruleSet: nameof(ILogic<T>.Delete));
        if (!valRes.IsValid)
        {
          return;
        }
      }

      _datastore.Delete(entity);
    }

    public virtual void Update(IEnumerable<T> entity)
    {
      foreach (var ent in entity)
      {
        var valRes = _validator.Validate(ent, ruleSet: nameof(ILogic<T>.Update));
        if (!valRes.IsValid)
        {
          return;
        }
      }

      _datastore.Update(entity);
    }
  }
}
