using System.Linq;

namespace GraphML.Interfaces
{
  public interface ILogic<T>
  {
    IQueryable<T> ByOwner(string ownerId);
    T Create(T entity);
    void Update(T entity);
    void Delete(T entity);
  }
}
