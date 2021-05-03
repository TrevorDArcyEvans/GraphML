using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IChartEdgeLogic : IOwnedLogic<ChartEdge>
  {
    IEnumerable<ChartEdge> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds);
  }
}
