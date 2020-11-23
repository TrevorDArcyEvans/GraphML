using System;
using System.Collections.Generic;
using GraphML.API.Server;

namespace GraphML.Interfaces.Server
{
  public interface IGraphServer : IOwnedItemServerBase<Graph>
  {
      IEnumerable<Graph> ByNodeId(Guid id);
      IEnumerable<Graph> ByEdgeId(Guid id);
  }
}
