using GraphML.Logic.Interfaces;
using GraphML.Utils;
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

    public virtual T Filter(T input)
    {
      // TODO   same org as contact OR HasRole(Admin)
      return input;
    }

    private T FilterInternal(T input)
    {
      if (input == null)
      {
        return input;
      }

      Verifier.Verify(input);

      return Filter(input);
    }

    public IEnumerable<T> Filter(IEnumerable<T> input)
    {
      return input.Select(x => FilterInternal(x)).Where(x => x != null);
    }
  }
}
