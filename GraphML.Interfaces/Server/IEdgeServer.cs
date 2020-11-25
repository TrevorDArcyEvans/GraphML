using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IEdgeServer : IRepositoryItemServer<Edge>
  {
    Task<IEnumerable<Edge>> ByNodeIds(IEnumerable<string> ids);
  }
}
