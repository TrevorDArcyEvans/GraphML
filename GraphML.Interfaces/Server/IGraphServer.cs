using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Server;

namespace GraphML.Interfaces.Server
{
  public interface IGraphServer : IOwnedItemServerBase<Graph>
  {
      Task<IEnumerable<Graph>> ByNodeId(Guid id);
      Task<IEnumerable<Graph>> ByEdgeId(Guid id);
  }
}
