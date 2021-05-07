using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IChartItemDatastore<T> : IOwnedDatastore<T> where T : ChartItem
  {
    IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> repoItemIds);
  }
}
