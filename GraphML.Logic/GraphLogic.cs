using FluentValidation;
using System;
using System.Collections.Generic;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GraphML.Logic
{
  public sealed class GraphLogic : OwnedLogicBase<Graph>, IGraphLogic
  {
    private readonly IGraphDatastore _graphDatastore;

    public GraphLogic(
      IHttpContextAccessor context,
      IGraphDatastore datastore,
      IGraphValidator validator,
      IGraphFilter filter) :
      base(context, datastore, validator, filter)
    {
      _graphDatastore = datastore;
    }

    public IEnumerable<Graph> ByEdgeId(Guid id, int pageIndex, int pageSize)
    {
      var valRes = _validator.Validate(new Graph(), options => options.IncludeRuleSets(nameof(ByEdgeId)));
      if (valRes.IsValid)
      {
        return _filter.Filter(_graphDatastore.ByEdgeId(id, pageIndex, pageSize));
      }

      return Enumerable.Empty<Graph>();
    }

    public IEnumerable<Graph> ByNodeId(Guid id, int pageIndex, int pageSize)
    {
      var valRes = _validator.Validate(new Graph(), options => options.IncludeRuleSets(nameof(ByNodeId)));
      if (valRes.IsValid)
      {
        return _filter.Filter(_graphDatastore.ByNodeId(id, pageIndex, pageSize));
      }

      return Enumerable.Empty<Graph>();
    }
  }
}
