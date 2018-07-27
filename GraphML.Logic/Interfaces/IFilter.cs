using System.Collections.Generic;

namespace GraphML.Logic.Interfaces
{
  public interface IFilter<T>
  {
    IEnumerable<T> Filter(IEnumerable<T> input);
  }
}
