using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface ITimelineNodeServer : IOwnedItemServerBase<TimelineNode>
  {
    Task<IEnumerable<TimelineNode>> ByGraphItems(Guid timelineId, IEnumerable<Guid> graphItems);
  }
}
