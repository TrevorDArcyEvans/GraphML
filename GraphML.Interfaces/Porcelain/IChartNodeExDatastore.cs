using System;
using System.Collections.Generic;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartNodeExDatastore
  {
    IEnumerable<ChartNodeEx> ByOwner(Guid chartId);
  }
}
