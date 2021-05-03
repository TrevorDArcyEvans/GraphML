using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IChartItemLogic<T> : IOwnedLogic<T> where T : ChartItem
  {
    IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds);
  }
}
