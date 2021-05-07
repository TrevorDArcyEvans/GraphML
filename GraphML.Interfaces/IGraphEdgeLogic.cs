using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IGraphEdgeLogic : IGraphItemLogic<GraphEdge>
  {
    PagedDataEx<GraphEdge> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm);
  }
}
