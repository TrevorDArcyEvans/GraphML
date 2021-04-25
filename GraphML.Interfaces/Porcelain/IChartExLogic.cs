using System;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartExLogic : IOwnedLogic<ChartEx>
  {
    ChartEx ById(Guid id);
  }
  public interface IChartNodeExLogic : IOwnedLogic<ChartNodeEx>
  {
  }
  public interface IChartEdgeExLogic : IOwnedLogic<ChartEdgeEx>
  {
  }
}
