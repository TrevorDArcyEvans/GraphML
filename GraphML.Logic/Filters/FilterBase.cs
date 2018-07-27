using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Logic.Filters
{
  public abstract class FilterBase<T> : IFilter<T>
  {
    protected readonly IHttpContextAccessor _context;

    public FilterBase(IHttpContextAccessor context)
    {
      _context = context;
    }

    protected virtual T Filter(T input)
    {
      return input;
    }

    public IEnumerable<T> Filter(IEnumerable<T> input)
    {
      return input.Select(x => Filter(x)).Where(x => x != null);
    }
  }
}
