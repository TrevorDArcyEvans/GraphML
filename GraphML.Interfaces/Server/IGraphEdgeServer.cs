using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IGraphEdgeServer : IOwnedItemServerBase<GraphEdge>
  {
    Task<IEnumerable<GraphEdge>> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds);
    Task<PagedDataEx<GraphEdge>> ByNodeIds(IEnumerable<Guid> ids, int pageIndex, int pageSize, string searchTerm);
    Task<IEnumerable<GraphEdge>> AddByFilter(Guid graphId, string filter);
  }
}
