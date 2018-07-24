using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
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

    protected abstract T Filter(T input);

    public IQueryable<T> Filter(IQueryable<T> input)
    {
      return input.Select(x => Filter(x)).Where(x => x != null);
    }
  }
}
