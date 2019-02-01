using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class RepositoryManagerLogic : LogicBase<RepositoryManager>, IRepositoryManagerLogic
  {
    public RepositoryManagerLogic(
      IHttpContextAccessor context,
      IRepositoryManagerDatastore datastore,
      IRepositoryManagerValidator validator,
      IRepositoryManagerFilter filter) :
      base(context, datastore, validator, filter)
    {
    }

    public IEnumerable<RepositoryManager> GetAll()
    {
      var valRes = _validator.Validate(new RepositoryManager(), ruleSet: nameof(IRepositoryManagerLogic.GetAll));
      if (valRes.IsValid)
      {
        return _filter.Filter(((IRepositoryManagerDatastore)_datastore).GetAll());
      }

      return Enumerable.Empty<RepositoryManager>();
    }
  }
}
