using System;
using GraphML.Interfaces.Porcelain;
using GraphML.Logic.Interfaces.Porcelain;
using GraphML.Porcelain;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Porcelain
{
  public sealed class ChartExLogic : OwnedLogicBase<ChartEx>, IChartExLogic
  {
    private readonly IChartExDatastore _chartExDatastore;
    
    public ChartExLogic(
      IHttpContextAccessor context,
      IChartExDatastore datastore,
      IChartExValidator validator,
      IChartExFilter filter) :
      base(context, datastore, validator, filter)
    {
      _chartExDatastore = datastore;
    }

    public ChartEx ById(Guid id)
    {
      // TODO   validation
      // TODO   filter
      return _chartExDatastore.ById(id);
    }
  }
  public sealed class ChartNodeExLogic : OwnedLogicBase<ChartNodeEx>, IChartNodeExLogic
  {
    public ChartNodeExLogic(
      IHttpContextAccessor context,
      IChartNodeExDatastore datastore,
      IChartNodeExValidator validator,
      IChartNodeExFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
  public sealed class ChartEdgeExLogic : OwnedLogicBase<ChartEdgeEx>, IChartEdgeExLogic
  {
    public ChartEdgeExLogic(
      IHttpContextAccessor context,
      IChartEdgeExDatastore datastore,
      IChartEdgeExValidator validator,
      IChartEdgeExFilter filter) :
      base(context, datastore, validator, filter)
    {
    }
  }
}
