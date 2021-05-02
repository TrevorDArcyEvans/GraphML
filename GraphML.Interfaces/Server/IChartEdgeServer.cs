using System;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IChartEdgeServer : IOwnedItemServerBase<ChartEdge>
  {
    // TODO   Task<ChartEdge> ByGraphItem(Guid chartId, Guid graphItem);
  }
}
