using GraphML.Common;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using GraphML.Utils;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace GraphML.Logic.Filters
{
  public abstract class FilterBase<T> : IFilter<T> where T : Item
  {
    protected readonly IHttpContextAccessor _context;
    protected readonly IContactDatastore _contactDatastore;

    public FilterBase(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore)
    {
      _context = context;
      _contactDatastore = contactDatastore;
    }

    public virtual T Filter(T input)
    {
      var email = _context.Email();
      var contact = _contactDatastore.ByEmail(email);
      var sameOrg = contact.OrganisationId == input.OrganisationId;

      return sameOrg || _context.HasRole(Roles.Admin) ? input : null;
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
