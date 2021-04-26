using System;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartExDatastore
  {
    ChartEx ById(Guid chartId);
  }
}
