using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.Interfaces.Server
{
  public interface IGraphItemServer<T> : IOwnedItemServerBase<T>
  {
    Task<IEnumerable<T>> ByRepositoryItems(Guid graphId, IEnumerable<Guid> repoItemIds);
    Task<IEnumerable<T>> AddByFilter(Guid graphId, string filter);
  }
}
