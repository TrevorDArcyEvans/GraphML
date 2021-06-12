using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IGraphEdgeServer : IGraphItemServer<GraphEdge>
  {
    Task<PagedDataEx<GraphEdge>> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm);
  }
}
