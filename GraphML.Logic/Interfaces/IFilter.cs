using System.Linq;

namespace GraphML.Logic.Interfaces
{
  public interface IFilter<T>
  {
    IQueryable<T> Filter(IQueryable<T> input);
  }
}
