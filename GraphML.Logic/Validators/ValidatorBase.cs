using FluentValidation;
using GraphML.Interfaces;
using GraphML.Common;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GraphML.Logic.Validators
{
  public abstract class ValidatorBase<T> : AbstractValidator<T>
  {
    protected readonly IHttpContextAccessor _context;
    protected readonly IContactDatastore _contactDatastore;
    private readonly IRoleDatastore _roleDatastore;

    public ValidatorBase(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore) :
      base()
    {
      _context = context;
      _contactDatastore = contactDatastore;
      _roleDatastore = roleDatastore;

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
        .Must(x => HasRole(Roles.Admin))
        .WithMessage("Must be admin");
    }

    protected void MustBeUserAdmin()
    {
      RuleFor(x => x)
        .Must(x => HasRole(Roles.UserAdmin))
        .WithMessage("Must be user admin");
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
