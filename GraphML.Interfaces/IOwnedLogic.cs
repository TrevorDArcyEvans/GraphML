using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IOwnedLogic<T> : ILogic<T>
  {
    IEnumerable<T> ByOwners(IEnumerable<string> ownerIds, int pageIndex, int pageSize);
  }
}
