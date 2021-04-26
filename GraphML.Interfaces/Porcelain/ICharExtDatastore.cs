using System;
using System.Collections.Generic;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartExDatastore
  {
    ChartEx ById(Guid chartId);
  }
  public interface IChartNodeExDatastore
  {
    IEnumerable<ChartNodeEx> ByOwner(Guid chartId);
  }
  public interface IChartEdgeExDatastore
  {
    IEnumerable<ChartEdgeEx> ByOwner(Guid chartId);
  }
}
