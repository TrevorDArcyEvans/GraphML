using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IEdgeDatastore : IRepositoryItemDatastore<Edge>
  {
    PagedDataEx<Edge> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize);
  }
}
