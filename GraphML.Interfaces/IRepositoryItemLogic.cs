using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IRepositoryItemLogic<T> : IOwnedLogic<T>
  {
    IEnumerable<T> GetParents(T entity, int pageIndex, int pageSize);
  }
}
