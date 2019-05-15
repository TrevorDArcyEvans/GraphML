using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public interface IEdgeServer : IOwnedItemServerBase<Edge>
  {
    Task<IEnumerable<Edge>> ByNodeIds(IEnumerable<string> ids);
  }
}
