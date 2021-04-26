using System;
using System.Collections.Generic;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartEdgeExDatastore
  {
    IEnumerable<ChartEdgeEx> ByOwner(Guid chartId);
  }
}
