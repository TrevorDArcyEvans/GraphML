using System;

namespace GraphML.Interfaces
{
  public interface IChartNodeLogic : IOwnedLogic<ChartNode>
  {
    ChartNode ByGraphItem(Guid chartId, Guid graphItemId);
  }
}
