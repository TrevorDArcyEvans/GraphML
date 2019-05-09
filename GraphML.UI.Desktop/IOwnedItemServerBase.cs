using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public interface IOwnedItemServerBase<T> : IItemServerBase<T>
  {
    IEnumerable<T> ByOwners(IEnumerable<string> ownerIds);
  }
}
