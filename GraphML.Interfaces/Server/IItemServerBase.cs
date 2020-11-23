using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphML.API.Server
{
  public interface IItemServerBase<T>
  {
    Task<IEnumerable<T>> ByIds(IEnumerable<Guid> ids);
    Task<IEnumerable<T>> Create(IEnumerable<T> entity);
    Task<IEnumerable<T>> Delete(IEnumerable<T> entity);
    Task<IEnumerable<T>> Update(IEnumerable<T> entity);
  }
}
