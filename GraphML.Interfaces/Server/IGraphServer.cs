using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IGraphServer : IOwnedItemServerBase<Graph>
  {
      Task<IEnumerable<Graph>> ByNodeId(Guid id, int pageIndex,	int pageSize);
      Task<IEnumerable<Graph>> ByEdgeId(Guid id, int pageIndex,	int pageSize);
  }
}
