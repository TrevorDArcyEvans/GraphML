using System.Linq;

namespace GraphML.Interfaces
{
  public interface IDatastore<T>
  {
    IQueryable<T> ByOwner(string ownerId);
    T Create(T entity);
    void Update(T entity);
    void Delete(T entity);
  }
}
