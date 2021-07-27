using System;
using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IItemDatastore<T>
  {
    IEnumerable<T> ByIds(IEnumerable<Guid> ids);
    IEnumerable<T> Create(IEnumerable<T> entity);
    void Update(IEnumerable<T> entity);
    void Delete(IEnumerable<T> entity);
    int Count();
  }
}
