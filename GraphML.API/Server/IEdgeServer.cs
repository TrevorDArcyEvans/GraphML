using System.Collections.Generic;

namespace GraphML.API.Server
{
  public interface IEdgeServer : IOwnedItemServerBase<Edge>
  {
    IEnumerable<Edge> ByNodeIds(IEnumerable<string> ids);
  }
}
