using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IOwnedDatastore<T> : IItemDatastore<T>
  {
    PagedDataEx<T> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex, int pageSize, string searchTerm);
    int Count(Guid ownerId);
  }
}
