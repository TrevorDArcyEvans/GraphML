using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Logic
{
  public abstract class OwnedLogicBase<T> : LogicBase<T>, IOwnedLogic<T> where T : OwnedItem, new()
  {
    public OwnedLogicBase(
      IHttpContextAccessor context, 
      IOwnedDatastore<T> datastore, 
      IValidator<T> validator, 
      IFilter<T> filter) : 
      base(context, datastore, validator, filter)
    {
    }

    public virtual IEnumerable<T> ByOwners(IEnumerable<string> ownerIds, int pageIndex, int pageSize)
    {
      var valRes = _validator.Validate(new T(), ruleSet: nameof(IOwnedLogic<T>.ByOwners));
      if (valRes.IsValid)
      {
        return _filter.Filter(((IOwnedDatastore<T>)_datastore).ByOwners(ownerIds, pageIndex, pageSize));
      }

      return Enumerable.Empty<T>();
    }
  }
}
