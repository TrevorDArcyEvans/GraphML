using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IGraphServer : IOwnedItemServerBase<Graph>
  {
      Task<PagedDataEx<Graph>> ByNodeId(Guid id, int pageIndex,	int pageSize, string searchTerm);
      Task<PagedDataEx<Graph>> ByEdgeId(Guid id, int pageIndex,	int pageSize, string searchTerm);
  }
}
