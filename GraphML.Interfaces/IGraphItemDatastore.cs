using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IGraphItemDatastore<T> : IOwnedDatastore<T> where T : GraphItem
  {
    IEnumerable<T> ByRepositoryItems(Guid graphId, IEnumerable<Guid> itemIds);
  }
}
