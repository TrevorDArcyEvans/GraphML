using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IChartNodeServer : IOwnedItemServerBase<ChartNode>
  {
    Task<IEnumerable<ChartNode>> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItems);
  }
}
