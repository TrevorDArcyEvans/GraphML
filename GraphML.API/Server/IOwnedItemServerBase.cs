using System.Collections.Generic;

namespace GraphML.API.Server
{
  public interface IOwnedItemServerBase<T> : IItemServerBase<T>
  {
    IEnumerable<T> ByOwners(IEnumerable<string> ownerIds);
  }
}
