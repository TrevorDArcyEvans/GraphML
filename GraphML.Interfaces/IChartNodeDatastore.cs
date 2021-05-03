using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IChartNodeDatastore : IOwnedDatastore<ChartNode>
  {
    IEnumerable<ChartNode> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds);
  }
}
