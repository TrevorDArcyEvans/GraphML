using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IEdgeDatastore : IOwnedDatastore<Edge>
  {
    IEnumerable<Edge> ByNodeIds(IEnumerable<string> ids);
  }
}
