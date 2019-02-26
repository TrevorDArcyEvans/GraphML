using FluentValidation;
using GraphML.Interfaces;
using GraphML.Common;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public abstract class ValidatorBase<T> : AbstractValidator<T>
  {
    protected readonly IHttpContextAccessor _context;

    public ValidatorBase(IHttpContextAccessor context) :
      base()
    {
      _context = context;
      RuleSet(nameof(ILogic<T>.Create), () =>
      {
        RuleForCreate();
      });
      RuleSet(nameof(ILogic<T>.Update), () =>
      {
        RuleForUpdate();
      });
      RuleSet(nameof(ILogic<T>.Delete), () =>
      {
        RuleForDelete();
      });
    }

    protected virtual void RuleForCreate()
    {
    }

    protected virtual void RuleForUpdate()
    {
    }

    protected virtual void RuleForDelete()
    {
    }

    protected void MustBeAdmin()
    {
      RuleFor(x => x)
        .Must(x => _context.HasRole(Roles.Admin))
        .WithMessage("Must be admin");
    }

    protected void MustBeUserAdmin()
    {
      RuleFor(x => x)
        .Must(x => _context.HasRole(Roles.UserAdmin))
        .WithMessage("Must be user admin");
    }
  }
}
