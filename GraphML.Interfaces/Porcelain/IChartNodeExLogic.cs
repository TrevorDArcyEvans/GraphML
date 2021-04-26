using System;
using System.Collections.Generic;
using GraphML.Porcelain;

namespace GraphML.Interfaces.Porcelain
{
  public interface IChartNodeExLogic
  {
    IEnumerable<ChartNodeEx> ByOwner(Guid chartId);
  }
}
