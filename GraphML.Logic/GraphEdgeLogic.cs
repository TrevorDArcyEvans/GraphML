using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GraphML.Logic
{
  public sealed class GraphEdgeLogic : GraphItemLogicBase<GraphEdge>, IGraphEdgeLogic
  {
    private readonly IGraphEdgeDatastore _edgeDatastore;

    public GraphEdgeLogic(
      IHttpContextAccessor context,
      ILogger<GraphEdgeLogic> logger,
      IGraphEdgeDatastore datastore,
      IGraphEdgeValidator validator,
      IGraphEdgeFilter filter) :
      base(context, logger, datastore, validator, filter)
    {
      _edgeDatastore = datastore;
    }

    public PagedDataEx<GraphEdge> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm)
    {
      var valRes = _validator.Validate(new GraphEdge(), options => options.IncludeRuleSets(nameof(IGraphEdgeLogic.ByNodeIds)));
      if (valRes.IsValid)
      {
        var pdex = _edgeDatastore.ByNodeIds(ids, pageIndex, pageSize, searchTerm);
        var filtered = _filter.Filter(pdex.Items);
        return new PagedDataEx<GraphEdge>
        {
          TotalCount = pdex.TotalCount,
          Items = filtered.ToList()
        };
      }

      _logger.LogError(valRes.ToString());
      return new PagedDataEx<GraphEdge>();
    }
  }
}
