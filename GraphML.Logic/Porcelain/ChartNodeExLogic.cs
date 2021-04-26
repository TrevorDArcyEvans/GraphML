using System;
using System.Collections.Generic;
using GraphML.Interfaces.Porcelain;
using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Porcelain
{
  public sealed class ChartNodeExLogic : IChartNodeExLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly IChartNodeExDatastore _datastore;
    private readonly IChartNodeExValidator _validator;
    private readonly IChartNodeExFilter _filter;

    public ChartNodeExLogic(
      IHttpContextAccessor context,
      IChartNodeExDatastore datastore,
      IChartNodeExValidator validator,
      IChartNodeExFilter filter)
    {
      _context = context;
      _datastore = datastore;
      _validator = validator;
      _filter = filter;
    }

    public IEnumerable<ChartNodeEx> ByOwner(Guid chartId)
    {
      // TODO   validation
      // TODO   filter
      return _filter.Filter(_datastore.ByOwner(chartId));
    }
  }
}
