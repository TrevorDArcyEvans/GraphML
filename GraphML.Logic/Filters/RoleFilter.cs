using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class RoleFilter : FilterBase<Role>, IRoleFilter
  {
    public RoleFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore) :
      base(context, contactDatastore)
    {
    }
  }
}
