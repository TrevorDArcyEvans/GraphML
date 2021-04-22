using FluentValidation;
using System;
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

    public PagedDataEx<Graph> ByEdgeId(Guid id, int pageIndex, int pageSize, string searchTerm)
    {
      var valRes = _validator.Validate(new Graph(), options => options.IncludeRuleSets(nameof(ByEdgeId)));
      if (valRes.IsValid)
      {
        var pdex = _graphDatastore.ByEdgeId(id, pageIndex, pageSize, searchTerm);
        var filtered = _filter.Filter(pdex.Items);
        return new PagedDataEx<Graph>
        {
          TotalCount = pdex.TotalCount,
          Items = filtered.ToList()
        };
      }

      return new PagedDataEx<Graph>();
    }

    public PagedDataEx<Graph> ByNodeId(Guid id, int pageIndex, int pageSize, string searchTerm)
    {
      var valRes = _validator.Validate(new Graph(), options => options.IncludeRuleSets(nameof(ByNodeId)));
      if (valRes.IsValid)
      {
        var pdex = _graphDatastore.ByNodeId(id, pageIndex, pageSize, searchTerm);
        var filtered = _filter.Filter(pdex.Items);
        return new PagedDataEx<Graph>
        {
          TotalCount = pdex.TotalCount,
          Items = filtered.ToList()
        };
      }

      return new PagedDataEx<Graph>();
    }
  }
}
