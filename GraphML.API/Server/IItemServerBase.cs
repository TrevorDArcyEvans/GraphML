using System.Collections.Generic;

namespace GraphML.API.Server
{
  public interface IItemServerBase<T>
  {
    IEnumerable<T> ByIds(IEnumerable<string> ids);
    IEnumerable<T> Create(IEnumerable<T> entity);
    IEnumerable<T> Delete(IEnumerable<T> entity);
    IEnumerable<T> Update(IEnumerable<T> entity);
  }
}
