using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface ITimelineItemLogic<T> : IOwnedLogic<T> where T : TimelineItem
  {
    IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> graphItemIds);
  }
}
