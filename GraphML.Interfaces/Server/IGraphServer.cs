using System;
using System.Collections.Generic;

namespace GraphML.API.Server
{
  public interface IGraphServer : IOwnedItemServerBase<Graph>
  {
      IEnumerable<Graph> ByNodeId(Guid id);
      IEnumerable<Graph> ByEdgeId(Guid id);
  }
}
