using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public interface IEdgeServer : IServerBase<Edge>
  {
    IEnumerable<Edge> ByNodeIds(IEnumerable<string> ids);
  }
}
