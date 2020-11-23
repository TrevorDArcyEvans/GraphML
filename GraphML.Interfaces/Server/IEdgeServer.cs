using System.Collections.Generic;
using System.Threading.Tasks;
using GraphML.API.Server;

namespace GraphML.Interfaces.Server
{
  public interface IEdgeServer : IOwnedItemServerBase<Edge>
  {
    Task<IEnumerable<Edge>> ByNodeIds(IEnumerable<string> ids);
  }
}
