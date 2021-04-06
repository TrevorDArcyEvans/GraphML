using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IRepositoryItemDatastore<T> : IOwnedDatastore<T>
  {
    IEnumerable<T> GetParents(Guid itemId, int pageIndex, int pageSize);
  }
}