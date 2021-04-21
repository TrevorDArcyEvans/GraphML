using System;

namespace GraphML.Interfaces
{
  public interface IRepositoryItemLogic<T> : IOwnedLogic<T>
  {
    PagedDataEx<T> GetParents(Guid itemId, int pageIndex, int pageSize);
  }
}
