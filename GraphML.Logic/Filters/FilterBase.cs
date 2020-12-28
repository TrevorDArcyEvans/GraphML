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
    protected readonly IRoleDatastore _roleDatastore;

    public FilterBase(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore)
    {
      _context = context;
      _contactDatastore = contactDatastore;
      _roleDatastore = roleDatastore;
    }

    public virtual T Filter(T input)
    {
      var email = _context.Email();
      var contact = _contactDatastore.ByEmail(email);
      var sameOrg = contact.OrganisationId == input.OrganisationId;

      return sameOrg || HasRole(Roles.Admin) ? input : null;
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

    private bool HasRole(string role)
    {
      var email = _context.Email();
      var contact = _contactDatastore.ByEmail(email);
      var roles = _roleDatastore.ByContactId(contact.Id);

      return roles.Any(x => x.Name == role);
    }
  }
}
