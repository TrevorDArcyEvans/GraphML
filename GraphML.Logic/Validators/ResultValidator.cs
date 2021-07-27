using System;
using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Common;
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
        RuleForByContact();
      });
      RuleSet(nameof(IResultLogic.ByOrganisation), () =>
      {
        RuleForByOrganisation();
      });
      RuleSet(nameof(IResultLogic.ByGraph), () =>
      {
        RuleForByGraph();
      });
      RuleSet(nameof(IResultLogic.ByCorrelation), () =>
      {
        RuleForByCorrelation();
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

    public void RuleForByContact()
    {
      // called by user
      //  Guid --> contactId
      RuleFor(x => x)
        .Must(x => MustBeSameContactOrUserAdmin(_context, x))
        .WithMessage("Must be same Contact");
    }

    public void RuleForByOrganisation()
    {
      // called by user
      //  Guid --> orgId
      RuleFor(x => x)
        .Must(x => MustBeSameOrganisation(_context, x))
        .WithMessage("Must be same Organisation");
    }

    public void RuleForByGraph()
    {
      // called by user
      //  Guid --> graphId
      // TODO   RuleForByGraph
    }

    public void RuleForByCorrelation()
    {
      // called by user
      //  Guid --> corrId
      // TODO   RuleForByCorrelation
    }

    public void RuleForRetrieve()
    {
      // called by user
      //  Guid --> correlationId
      RuleFor(x => x)
        .Must(x => MustBeSameContactAsRequestOrUserAdmin(_context, x))
        .WithMessage("Must be same Contact as Request");
    }

    public void RuleForDelete()
    {
      // called by user
      //  Guid --> correlationId
      RuleFor(x => x)
        .Must(x => MustBeSameContactAsRequestOrUserAdmin(_context, x))
        .WithMessage("Must be same Contact as Request");
    }

    public bool MustBeSameContactOrUserAdmin(IHttpContextAccessor context, Guid contactId)
    {
      var reqEmail = context.Email();
      var reqContact = _contactDatastore.ByEmail(reqEmail);
      var contact = _contactDatastore.ByIds(new []{ contactId }).SingleOrDefault();

      return reqContact.Id == contactId || // requester must be asking for his results OR
        (IsUserAdmin(contact.Id) && // requester must be UserAdmin
        reqContact.OrganisationId == contact.OrganisationId); // for same org as contact
    }

    public bool MustBeSameOrganisation(IHttpContextAccessor context, Guid orgId)
    {
      // requester must be in same org as results
      var reqEmail = context.Email();
      var reqContact = _contactDatastore.ByEmail(reqEmail);

      return reqContact.OrganisationId == orgId;
    }

    public bool MustBeSameContactAsRequestOrUserAdmin(IHttpContextAccessor context, Guid correlationId)
    {
      var reqEmail = context.Email();
      var reqContact = _contactDatastore.ByEmail(reqEmail);
      var matchRequest = _resultDatastore
        .ByContact(reqContact.Id)
        .Any(x =>
          x.CorrelationId == correlationId &&
          x.Contact.Id == reqContact.Id);
      var corrRequest = _resultDatastore.ByCorrelation(correlationId);

      return matchRequest || // requester must be asking for his result OR
        (IsUserAdmin(reqContact.Id) &&  // requester must be UserAdmin
        corrRequest.Contact.OrganisationId == reqContact.OrganisationId); // for same org as request
    }

    public bool IsUserAdmin(Guid contactId)
    {
      var roles = _roleDatastore.ByContactId(contactId);

      return roles.Any(x => x.Name == Roles.UserAdmin);
    }
  }
}
