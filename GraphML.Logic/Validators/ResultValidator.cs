using System;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class ResultValidator : ValidatorBase<Guid>, IResultValidator
    {
      public ResultValidator(
        IHttpContextAccessor context,
        IContactDatastore contactDatastore,
        IRoleDatastore roleDatastore) :
        base(context, contactDatastore, roleDatastore)
        {
            RuleSet(nameof(IResultLogic.List), () =>
            {
            });
            RuleSet(nameof(IResultLogic.Retrieve), () =>
            {
            });
        }

        protected override void RuleForDelete()
        {
            MustBeAdmin();
        }
    }
}
