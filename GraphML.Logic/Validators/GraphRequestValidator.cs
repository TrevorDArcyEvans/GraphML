using System.Linq;
using FluentValidation;
using GraphML.Common;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphRequestValidator : AbstractValidator<IGraphRequest>, IGraphRequestValidator
  {
    private readonly IHttpContextAccessor _context;
    private readonly IContactDatastore _contactDatastore;
    private readonly IRoleDatastore _roleDatastore;
    private readonly IGraphDatastore _graphDatastore;

    public GraphRequestValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore,
      IGraphDatastore graphDatastore)
    {
      _context = context;
      _contactDatastore = contactDatastore;
      _roleDatastore = roleDatastore;
      _graphDatastore = graphDatastore;

      RuleSet(nameof(IAnalysisLogic.Degree), () =>
      {
        RequesterMustBeSameOrganisation();
      });
      RuleSet(nameof(IAnalysisLogic.Closeness), () =>
      {
        RequesterMustBeSameOrganisation();
      });
      RuleSet(nameof(IAnalysisLogic.Betweenness), () =>
      {
        RequesterMustBeSameOrganisation();
      });
      RuleSet(nameof(IAnalysisLogic.FindShortestPaths), () =>
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

    public bool RequesterIsSameOrganisation(IHttpContextAccessor context, IGraphRequest graphRequest)
    {
      var email = context.Email();
      var contact = _contactDatastore.ByEmail(email);
      var graph = _graphDatastore.ByIds(new[] { graphRequest.GraphId }).Single();

      return contact.OrganisationId == graph.OrganisationId;
    }
  }
}
