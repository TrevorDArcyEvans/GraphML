using System;

namespace GraphML.Interfaces
{
  public interface IChartNodeDatastore : IOwnedDatastore<ChartNode>
  {
    ChartNode ByGraphItem(Guid chartId, Guid graphItemId);
  }
}
