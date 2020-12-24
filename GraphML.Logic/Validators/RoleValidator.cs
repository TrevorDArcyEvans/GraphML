using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class RoleValidator : ValidatorBase<Role>, IRoleValidator
    {
      public RoleValidator(
        IHttpContextAccessor context,
        IContactDatastore contactDatastore,
        IRoleDatastore roleDatastore) :
        base(context, contactDatastore, roleDatastore)
        {
            RuleSet(nameof(IRoleLogic.ByContactId), () =>
            {
            });
            RuleSet(nameof(IRoleLogic.GetAll), () =>
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