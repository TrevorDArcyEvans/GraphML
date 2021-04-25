using System;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartExDatastore : IOwnedDatastore<ChartEx>
  {
    ChartEx ById(Guid id);
  }
  public interface IChartNodeExDatastore : IOwnedDatastore<ChartNodeEx>
  {
  }
  public interface IChartEdgeExDatastore : IOwnedDatastore<ChartEdgeEx>
  {
  }
}
