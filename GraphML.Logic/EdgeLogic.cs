using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class EdgeLogic : RepositoryItemLogic<Edge>, IEdgeLogic
  {
    private readonly IEdgeDatastore _edgeDatastore;

    public EdgeLogic(
      IHttpContextAccessor context,
      ILogger<EdgeLogic> logger,
      IEdgeDatastore datastore,
      IEdgeValidator validator,
      IEdgeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
      _edgeDatastore = datastore;
    }

    public PagedDataEx<Edge> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm)
    {
      var valRes = _validator.Validate(new Edge(), options => options.IncludeRuleSets(nameof(IEdgeLogic.ByNodeIds)));
      if (valRes.IsValid)
      {
        var pdex = _edgeDatastore.ByNodeIds(ids, pageIndex, pageSize, searchTerm);
        var filtered = _filter.Filter(pdex.Items);
        return new PagedDataEx<Edge>
        {
          TotalCount = pdex.TotalCount,
          Items = filtered.ToList()
        };
      }

      _logger.LogError(valRes.ToString());
      return new PagedDataEx<Edge>();
    }
  }
}
