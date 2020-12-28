using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class RepositoryItemAttributeFilter : FilterBase<RepositoryItemAttribute>, IRepositoryItemAttributeFilter
  {
    public RepositoryItemAttributeFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)

    {
    }
  }
}
