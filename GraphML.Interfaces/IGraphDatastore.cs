using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IGraphDatastore : IOwnedDatastore<Graph>
  {
    IEnumerable<Graph> ByNodeId(Guid id, int pageIndex, int pageSize);

    IEnumerable<Graph> ByEdgeId(Guid id, int pageIndex, int pageSize);
  }
}
