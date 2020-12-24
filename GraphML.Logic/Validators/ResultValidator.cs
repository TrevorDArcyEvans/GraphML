using System;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class ResultValidator : ValidatorBase<Guid>, IResultValidator
  {
    private readonly IResultDatastore _resultDatastore;

    public ResultValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore, 
      IResultDatastore resultDatastore) :
      base(context, contactDatastore, roleDatastore)
    {
      _resultDatastore = resultDatastore;

      RuleSet(nameof(IResultLogic.List), () =>
      {
        RuleForList();
      });
      RuleSet(nameof(IResultLogic.Retrieve), () =>
      {
        RuleForRetrieve();
      });
    }

    public void RuleForList()
    {
      // called by user
      //  Guid --> contactId
      RuleFor(x => x)
        .Must(x => MustBeSameContact(_context, x))
        .WithMessage("Must be same Contact");
    }

    public void RuleForRetrieve()
    {
      // called by user
      //  Guid --> correlationId
      RuleFor(x => x)
        .Must(x => MustBeSameContactAsRequest(_context, x))
        .WithMessage("Must be same Contact as Request");
    }

    protected override void RuleForDelete()
    {
      // called by user
      //  Guid --> correlationId
      RuleFor(x => x)
        .Must(x => MustBeSameContactAsRequest(_context, x))
        .WithMessage("Must be same Contact as Request");
    }

    private bool MustBeSameContact(IHttpContextAccessor context, Guid contactId)
    {
			var email = context.Email();
			var contact = _contactDatastore.ByEmail(email);

      return contact.Id == contactId;
    }

    private bool MustBeSameContactAsRequest(IHttpContextAccessor context, Guid correlationId)
    {
			var email = context.Email();
			var contact = _contactDatastore.ByEmail(email);
      var matchRequest = _resultDatastore
        .List(contact.Id)
        .Any(x => 
          x.CorrelationId == correlationId && 
          x.Contact.Id == contact.Id);

      return matchRequest;
    }
  }
}
