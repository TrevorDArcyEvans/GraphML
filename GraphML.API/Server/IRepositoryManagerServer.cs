using System.Collections.Generic;

namespace GraphML.API.Server
{
  public interface IRepositoryManagerServer : IOwnedItemServerBase<RepositoryManager>
  {
    IEnumerable<RepositoryManager> GetAll();
  }
}
