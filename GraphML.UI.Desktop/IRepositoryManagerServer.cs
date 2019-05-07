using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public interface IRepositoryManagerServer : IServerBase<RepositoryManager>
  {
    IEnumerable<RepositoryManager> GetAll();
  }
}
