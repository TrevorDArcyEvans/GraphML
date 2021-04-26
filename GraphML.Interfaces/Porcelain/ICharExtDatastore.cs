using System;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartExDatastore
  {
    ChartEx ById(Guid chartId);
  }
  public interface IChartNodeExDatastore : IOwnedDatastore<ChartNodeEx>
  {
  }
  public interface IChartEdgeExDatastore : IOwnedDatastore<ChartEdgeEx>
  {
  }
}
