using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IGraphNodeServer : IOwnedItemServerBase<GraphNode>
  {
    Task<IEnumerable<GraphNode>> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds);
    Task<IEnumerable<GraphNode>> AddByFilter(Guid graphId, string filter);
  }
}
