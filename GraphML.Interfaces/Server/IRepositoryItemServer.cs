using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IRepositoryItemServer<T> : IOwnedItemServerBase<T>
  {
    Task<PagedDataEx<T>> GetParents(Guid itemId, int pageIndex, int pageSize, string searchTerm);
  }
}
