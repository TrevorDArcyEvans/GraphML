using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IRepositoryManagerLogic : ILogic<RepositoryManager>
  {
    IEnumerable<RepositoryManager> GetAll();
  }
}
