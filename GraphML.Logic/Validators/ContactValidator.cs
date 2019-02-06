using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class ContactValidator : OwnedValidatorBase<Contact>, IContactValidator
  {
    public ContactValidator(IHttpContextAccessor context) :
      base(context)
    {
      RuleSet(nameof(IContactLogic.ByEmail), () =>
      {
      });
    }

    protected override void RuleForCreate()
    {
      MustBeAdmin();
    }

    protected override void RuleForUpdate()
    {
      MustBeAdmin();
    }

    protected override void RuleForDelete()
    {
      MustBeAdmin();
    }
  }
}
