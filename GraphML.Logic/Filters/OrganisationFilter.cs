using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class OrganisationFilter : FilterBase<Organisation>, IOrganisationFilter
  {
    public OrganisationFilter(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore) :
      base(context, contactDatastore)
    {
    }
  }
}
