using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface IDatastore<T>
  {
    IEnumerable<T> ByIds(IEnumerable<string> id);
    IEnumerable<T> Create(IEnumerable<T> entity);
    void Update(IEnumerable<T> entity);
    void Delete(IEnumerable<T> entity);
  }
}
