using System;
using System.Collections.Generic;
using GraphML.Interfaces.Porcelain;
using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Porcelain
{
  public sealed class ChartEdgeExLogic : IChartEdgeExLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly IChartEdgeExDatastore _datastore;
    private readonly IChartEdgeExValidator _validator;
    private readonly IChartEdgeExFilter _filter;

    public ChartEdgeExLogic(
      IHttpContextAccessor context,
      IChartEdgeExDatastore datastore,
      IChartEdgeExValidator validator,
      IChartEdgeExFilter filter)
    {
      _context = context;
      _datastore = datastore;
      _validator = validator;
      _filter = filter;
    }

    public IEnumerable<ChartEdgeEx> ByOwner(Guid chartId)
    {
      // TODO   validation
      // TODO   filter
      return _filter.Filter(_datastore.ByOwner(chartId));
    }
  }
}
