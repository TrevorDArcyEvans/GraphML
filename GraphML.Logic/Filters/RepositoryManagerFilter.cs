using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace GraphML.Logic.Filters
{
  public sealed class RepositoryManagerFilter : FilterBase<RepositoryManager>, IRepositoryManagerFilter
  {
    public RepositoryManagerFilter(IHttpContextAccessor context) :
      base(context)
    {
    }

    protected override RepositoryManager Filter(RepositoryManager input)
    {
      throw new NotImplementedException();
    }
  }
}
