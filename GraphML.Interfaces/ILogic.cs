using System.Collections.Generic;

namespace GraphML.Interfaces
{
  public interface ILogic<T>
  {
    IEnumerable<T> Ids(IEnumerable<string> ids);
    IEnumerable<T> ByOwners(IEnumerable<string> ownerIds);
    IEnumerable<T> Create(IEnumerable<T> entity);
    void Update(IEnumerable<T> entity);
    void Delete(IEnumerable<T> entity);
  }
}
