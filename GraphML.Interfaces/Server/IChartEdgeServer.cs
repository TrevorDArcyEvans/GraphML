using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IChartEdgeServer : IOwnedItemServerBase<ChartEdge>
  {
    Task<IEnumerable<ChartEdge>> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItems);
  }
}
