using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IOwnedDatastore<T> : IDatastore<T>
  {
    IEnumerable<T> ByOwners(IEnumerable<string> ownerIds, int pageIndex, int pageSize);
  }
}
