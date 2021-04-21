using System;

namespace GraphML.Interfaces
{
  public interface IRepositoryItemDatastore<T> : IOwnedDatastore<T>
  {
    PagedDataEx<T> GetParents(Guid itemId, int pageIndex, int pageSize);
  }
}