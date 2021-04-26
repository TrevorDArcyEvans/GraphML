using System;
using System.Collections.Generic;
using GraphML.Interfaces.Porcelain;
using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Porcelain
{
  public sealed class ChartExLogic : IChartExLogic
  {
    private readonly IHttpContextAccessor _context;
    private readonly IChartExDatastore _chartExDatastore;
    private readonly IChartExValidator _validator;
    private readonly IChartExFilter _filter;

    public ChartExLogic(
      IHttpContextAccessor context,
      IChartExDatastore datastore,
      IChartExValidator validator,
      IChartExFilter filter)
    {
      _context = context;
      _chartExDatastore = datastore;
      _validator = validator;
      _filter = filter;
    }

    public ChartEx ById(Guid id)
    {
      // TODO   validation
      // TODO   filter
      return _chartExDatastore.ById(id);
    }
  }

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
