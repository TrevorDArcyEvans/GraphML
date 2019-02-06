using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Filters
{
  public sealed class ContactFilter : FilterBase<Contact>, IContactFilter
  {
    public ContactFilter(IHttpContextAccessor context) :
      base(context)
    {
    }
  }
}
