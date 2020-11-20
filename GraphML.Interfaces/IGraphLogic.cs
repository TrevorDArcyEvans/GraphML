using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IGraphLogic : IOwnedLogic<Graph>
  {
    IEnumerable<Graph> ByNodeId(Guid id, int pageIndex, int pageSize);

    IEnumerable<Graph> ByEdgeId(Guid id, int pageIndex, int pageSize);
  }
}
