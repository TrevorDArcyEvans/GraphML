using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace GraphML.Logic.Filters
{
  public sealed class RepositoryFilter : FilterBase<Repository>, IRepositoryFilter
  {
    public RepositoryFilter(IHttpContextAccessor context) :
      base(context)
    {
    }

    protected override Repository Filter(Repository input)
    {
      throw new NotImplementedException();
    }
  }
}
