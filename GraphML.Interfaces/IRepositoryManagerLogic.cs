using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IRepositoryManagerLogic : IOwnedLogic<RepositoryManager>
  {
    IEnumerable<RepositoryManager> GetAll();
  }
}
