using FluentValidation;
using System.Linq;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic
{
  public sealed class ContactLogic : OwnedLogicBase<Contact>, IContactLogic
  {
    public ContactLogic(
      IHttpContextAccessor context,
      IContactDatastore datastore,
      IContactValidator validator,
      IContactFilter filter) :
      base(context, datastore, validator, filter)
    {
    }

    public Contact ByEmail(string email)
    {
      var valRes = _validator.Validate(new Contact(), ruleSet: nameof(IContactLogic.ByEmail));
      if (valRes.IsValid)
      {
        return _filter.Filter(new[] { ((IContactDatastore)_datastore).ByEmail(email) }).SingleOrDefault();
      }

      return null;
    }
  }
}
