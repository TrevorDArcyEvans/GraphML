using System.Collections.Generic;

namespace GraphML.UI.Desktop
{
  public interface IServerBase<T>
  {
    IEnumerable<T> ByIds(IEnumerable<string> ids);
    IEnumerable<T> ByOwners(IEnumerable<string> ownerIds);
    IEnumerable<T> Create(IEnumerable<T> entity);
    IEnumerable<T> Delete(IEnumerable<T> entity);
    IEnumerable<T> Update(IEnumerable<T> entity);
  }
}
