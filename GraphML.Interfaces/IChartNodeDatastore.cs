﻿using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IChartNodeDatastore : IChartItemDatastore<ChartNode>
  {
  }

  public interface IChartItemDatastore<T> : IOwnedDatastore<T> where T : ChartItem
  {
    IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds);
  }
}
