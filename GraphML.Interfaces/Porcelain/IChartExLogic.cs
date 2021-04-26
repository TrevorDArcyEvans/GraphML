using System;
using System.Collections.Generic;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartExLogic
  {
    ChartEx ById(Guid id);
  }
  public interface IChartNodeExLogic
  {
    IEnumerable<ChartNodeEx> ByOwner(Guid chartId);
  }
  public interface IChartEdgeExLogic
  {
    IEnumerable<ChartEdgeEx> ByOwner(Guid chartId);
  }
}
