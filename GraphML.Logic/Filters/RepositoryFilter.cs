using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class RepositoryFilter : FilterBase<Repository>, IRepositoryFilter
  {
    public RepositoryFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
