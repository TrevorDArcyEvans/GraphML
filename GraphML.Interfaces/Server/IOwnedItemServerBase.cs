using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IOwnedItemServerBase<T> : IItemServerBase<T>
  {
    Task<PagedDataEx<T>> ByOwners(IEnumerable<Guid> ownerIds, int pageIndex, int pageSize, string searchTerm);
    Task<PagedDataEx<T>> ByOwner(Guid ownerId, int pageIndex, int pageSize, string searchTerm);
    Task<int> Count(Guid ownerId);
  }
}
