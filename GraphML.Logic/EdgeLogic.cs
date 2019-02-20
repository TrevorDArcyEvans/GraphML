using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class EdgeLogic : OwnedLogicBase<Edge>, IEdgeLogic
  {
    public EdgeLogic(
      IHttpContextAccessor context,
      IEdgeDatastore datastore,
      IEdgeValidator validator,
      IEdgeFilter filter) :
      base(context, datastore, validator, filter)
    {
    }

    public IEnumerable<Edge> ByNodeIds(IEnumerable<string> ids, int pageIndex, int pageSize)
    {
      var valRes = _validator.Validate(new Edge(), ruleSet: nameof(IEdgeLogic.ByNodeIds));
      if (valRes.IsValid)
      {
        return _filter.Filter(((IEdgeDatastore)_datastore).ByNodeIds(ids, pageIndex, pageSize));
      }

      return Enumerable.Empty<Edge>();
    }
  }
}
