using System.Linq;
using FluentValidation;
using GraphML.Common;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
  public sealed class GraphNodesRequestValidator : AbstractValidator<IGraphNodesRequest>, IGraphNodesRequestValidator
  {
    private readonly IHttpContextAccessor _context;
    private readonly IContactDatastore _contactDatastore;
    private readonly IRoleDatastore _roleDatastore;
    private readonly IGraphDatastore _graphDatastore;
    private readonly IGraphNodeDatastore _graphNodeDatastore;

    public GraphNodesRequestValidator(
      IHttpContextAccessor context,
      IContactDatastore contactDatastore,
      IRoleDatastore roleDatastore,
      IGraphDatastore graphDatastore,
      IGraphNodeDatastore graphNodeDatastore)
    {
      _context = context;
      _contactDatastore = contactDatastore;
      _roleDatastore = roleDatastore;
      _graphDatastore = graphDatastore;
      _graphNodeDatastore = graphNodeDatastore;

      RuleSet(nameof(IAnalysisLogic.FindShortestPaths), () =>
      {
        RequesterMustBeSameOrganisation();
        MustBeInSameGraph();
      });
    }

    public void RequesterMustBeSameOrganisation()
    {
      RuleFor(x => x)
        .Must(x => RequesterIsSameOrganisation(_context, x))
        .WithMessage("Must be in same Organisation");
    }

    public void MustBeInSameGraph()
    {
      RuleFor(x => x)
        .Must(x => AreInSameGraph(x))
        .WithMessage("Must be in same Graph");
    }

    public bool RequesterIsSameOrganisation(IHttpContextAccessor context, IGraphNodesRequest graphNodesRequest)
    {
      var email = context.Email();
      var contact = _contactDatastore.ByEmail(email);

      var graphNodes = _graphNodeDatastore.ByIds(graphNodesRequest.GraphNodes);
      var graph = _graphDatastore.ByIds(graphNodes.Select(gn => gn.GraphId)).Distinct().SingleOrDefault();
      if (graph == null)
      {
        return false;
      }

      return contact.OrganisationId == graph.OrganisationId;
    }

    public bool AreInSameGraph(IGraphNodesRequest graphNodesRequest)
    {
      var graphNodes = _graphNodeDatastore.ByIds(graphNodesRequest.GraphNodes);
      var numGraph = _graphDatastore.ByIds(graphNodes.Select(gn => gn.GraphId)).Distinct().Count();

      return numGraph == 1;
    }
  }
}
