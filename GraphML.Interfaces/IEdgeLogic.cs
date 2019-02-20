using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IEdgeLogic : IOwnedLogic<Edge>
  {
    IEnumerable<Edge> ByNodeIds(IEnumerable<string> ids, int pageIndex, int pageSize);
  }
}
