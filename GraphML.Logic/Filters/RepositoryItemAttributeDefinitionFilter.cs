using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class RepositoryItemAttributeDefinitionFilter : FilterBase<RepositoryItemAttributeDefinition>, IRepositoryItemAttributeDefinitionFilter
  {
    public RepositoryItemAttributeDefinitionFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
    }
  }
}
