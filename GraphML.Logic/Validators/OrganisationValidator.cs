using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class OrganisationValidator : ValidatorBase<Organisation>, IOrganisationValidator
  {
    public OrganisationValidator(IHttpContextAccessor context) :
      base(context)
    {
      RuleSet(nameof(IOrganisationLogic.GetAll), () =>
      {
        MustBeAdmin();
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
