using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public interface IOwnedItemServerBase<T> : IItemServerBase<T>
  {
    Task<IEnumerable<T>> ByOwners(IEnumerable<Guid> ownerIds);
  }
}
