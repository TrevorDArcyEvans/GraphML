using System;
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
}
