using FluentValidation;
using System.Linq;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ContactLogic : OwnedLogicBase<Contact>, IContactLogic
  {
    private readonly IContactDatastore _contactDatastore;

    public ContactLogic(
      IHttpContextAccessor context,
      IContactDatastore datastore,
      IContactValidator validator,
      IContactFilter filter) :
      base(context, datastore, validator, filter)
    {
      _contactDatastore = datastore;
    }

    public Contact ByEmail(string email)
    {
      var valRes = _validator.Validate(new Contact(), options => options.IncludeRuleSets(nameof(IContactLogic.ByEmail)));
      if (valRes.IsValid)
      {
        return _filter.Filter(new[] { _contactDatastore.ByEmail(email) }).SingleOrDefault();
      }

      return null;
    }
  }
}
