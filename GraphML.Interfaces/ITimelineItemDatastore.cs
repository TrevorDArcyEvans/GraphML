using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface ITimelineItemDatastore<T> : IOwnedDatastore<T> where T : TimelineItem
  {
    IEnumerable<T> ByGraphItems(Guid chartId, IEnumerable<Guid> repoItemIds);
  }
}
