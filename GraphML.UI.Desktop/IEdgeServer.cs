using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public interface IEdgeServer : IOwnedItemServerBase<Edge>
  {
    IEnumerable<Edge> ByNodeIds(IEnumerable<string> ids);
  }
}
