using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public interface IRepositoryManagerServer : IOwnedItemServerBase<RepositoryManager>
  {
    IEnumerable<RepositoryManager> GetAll();
  }
}
