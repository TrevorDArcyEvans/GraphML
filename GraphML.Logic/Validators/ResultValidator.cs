using System;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class ResultValidator : AbstractValidator<Guid>, IResultValidator
  {
    private readonly IHttpContextAccessor _context;
    private readonly IContactDatastore _contactDatastore;
    private readonly IRoleDatastore _roleDatastore;
    private readonly IResultDatastore _resultDatastore;

    public ResultValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore,
      IResultDatastore resultDatastore)
    {
      _context = context;
      _contactDatastore = contactDatastore;
      _roleDatastore = roleDatastore;
      _resultDatastore = resultDatastore;

      RuleSet(nameof(IResultLogic.Create), () =>
      {
        RuleForCreate();
      });
      RuleSet(nameof(IResultLogic.ByContact), () =>
      {
        RuleForList();
      });
      RuleSet(nameof(IResultLogic.Retrieve), () =>
      {
        RuleForRetrieve();
      });
      RuleSet(nameof(IResultLogic.Delete), () =>
      {
        RuleForDelete();
      });
    }

    public void RuleForCreate()
    {
      // called by Analysis.Server
      //  Guid --> CorrelationId
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

    public void RuleForDelete()
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
        .ByContact(contact.Id)
        .Any(x =>
          x.CorrelationId == correlationId &&
          x.Contact.Id == contact.Id);

      return matchRequest;
    }
  }
}
