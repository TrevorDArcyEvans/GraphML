using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface ITimelineEdgeServer : IOwnedItemServerBase<TimelineEdge>
  {
    Task<IEnumerable<TimelineEdge>> ByGraphItems(Guid timelineId, IEnumerable<Guid> graphItems);
  }
}
