using System;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IChartNodeServer : IOwnedItemServerBase<ChartNode>
  {
    Task<ChartNode> ByGraphItem(Guid chartId, Guid graphItem);
  }
}
