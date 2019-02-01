using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IRepositoryManagerDatastore : IDatastore<RepositoryManager>
  {
    IEnumerable<RepositoryManager> GetAll();
  }
}
