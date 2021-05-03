using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IChartEdgeDatastore : IOwnedDatastore<ChartEdge>
  {
    IEnumerable<ChartEdge> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds);
  }
}
