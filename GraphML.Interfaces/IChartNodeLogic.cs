using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IChartNodeLogic : IOwnedLogic<ChartNode>
  {
    IEnumerable<ChartNode> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds);
  }
}
