using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IEdgeServer : IRepositoryItemServer<Edge>
  {
    Task<PagedDataEx<Edge>> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm);
  }
}
