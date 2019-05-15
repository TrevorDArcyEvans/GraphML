using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public interface IRepositoryManagerServer : IOwnedItemServerBase<RepositoryManager>
  {
    Task<IEnumerable<RepositoryManager>> GetAll();
  }
}
