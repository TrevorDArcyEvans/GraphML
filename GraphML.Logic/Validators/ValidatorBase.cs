using FluentValidation;
using GraphML.API;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public abstract class ValidatorBase<T> : AbstractValidator<T>
  {
    protected readonly IHttpContextAccessor _context;

    public ValidatorBase(IHttpContextAccessor context)
    {
      _context = context;
    }

    protected void MustBeAdmin()
    {
      RuleFor(x => x)
        .Must(x => _context.HasRole(Roles.Admin))
        .WithMessage("Must be admin");
    }
  }
}
