using System;
using System.Linq;
using FluentValidation;
using GraphML.Datastore.Database.Importer;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class ImporterValidator : AbstractValidator<ImportSpecification>, IImporterValidator
  {
    private readonly IHttpContextAccessor _context;
    private readonly IContactDatastore _contactDatastore;
    private readonly IOrganisationDatastore _orgDatastore;

    public ImporterValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore,
      IOrganisationDatastore orgDatastore)
    {
      _context = context;
      _contactDatastore = contactDatastore;
      _orgDatastore = orgDatastore;

      RuleSet(nameof(IImporterLogic.Import), () =>
      {
        RequesterMustBeSameOrganisation();
      });
    }

    public void RequesterMustBeSameOrganisation()
    {
      RuleFor(x => x)
        .Must(x => RequesterIsSameOrganisation(_context, x))
        .WithMessage("Must be in same Organisation");
    }

    public bool RequesterIsSameOrganisation(IHttpContextAccessor context, ImportSpecification importSpec)
    {
      var email = context.Email();
      var contact = _contactDatastore.ByEmail(email);

      var org = _orgDatastore.GetAll()
        .SingleOrDefault(x => string.Equals(x.Name, importSpec.Organisation, StringComparison.InvariantCultureIgnoreCase));

      return contact.OrganisationId == org?.Id;
    }
  }
}
