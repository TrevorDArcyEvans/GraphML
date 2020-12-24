using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
	public sealed class GraphNodesRequestValidator : ValidatorBase<IGraphNodesRequest>, IGraphNodesRequestValidator
	{
		private readonly IGraphDatastore _graphDatastore;
		private readonly IGraphNodeDatastore _graphNodeDatastore;
		private readonly IRepositoryDatastore _repositoryDatastore;
		private readonly IRepositoryManagerDatastore _repositoryManagerDatastore;
		private readonly IOrganisationDatastore _organisationDatastore;

		public GraphNodesRequestValidator(
		  IHttpContextAccessor context,
		  IContactDatastore contactDatastore,
		  IRoleDatastore roleDatastore,
	  IGraphDatastore graphDatastore,
		  IGraphNodeDatastore graphNodeDatastore,
		  IRepositoryDatastore repositoryDatastore,
		  IRepositoryManagerDatastore repositoryManagerDatastore,
		  IOrganisationDatastore organisationDatastore) :
		  base(context, contactDatastore, roleDatastore)
		{
			_graphDatastore = graphDatastore;
			_graphNodeDatastore = graphNodeDatastore;
			_repositoryDatastore = repositoryDatastore;
			_repositoryManagerDatastore = repositoryManagerDatastore;
			_organisationDatastore = organisationDatastore;

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
			throw new System.NotImplementedException();
		}

		public void MustBeInSameGraph()
		{
			RuleFor(x => x)
			  .Must(x => AreInSameGraph(x))
			  .WithMessage("Must be in same Graph");
		}

		private bool RequesterIsSameOrganisation(IHttpContextAccessor context, IGraphNodesRequest graphNodesRequest)
		{
			var email = context.Email();
			var contact = _contactDatastore.ByEmail(email);

			var graphNodes = _graphNodeDatastore.ByIds(graphNodesRequest.GraphNodes);
			var graph = _graphDatastore.ByIds(graphNodes.Select(gn => gn.GraphId)).SingleOrDefault();
			if (graph == null)
			{
				return false;
			}
			var repo = _repositoryDatastore.ByIds(new[] { graph.RepositoryId }).Single();
			var repoMgr = _repositoryManagerDatastore.ByIds(new[] { repo.RepositoryManagerId }).Single();
			var org = _organisationDatastore.ByIds(new[] { repoMgr.OrganisationId }).Single();

			return contact.OrganisationId == org.Id;
		}

		private bool AreInSameGraph(IGraphNodesRequest graphNodesRequest)
		{
			var graphNodes = _graphNodeDatastore.ByIds(graphNodesRequest.GraphNodes);
			var numGraph = _graphDatastore.ByIds(graphNodes.Select(gn => gn.GraphId)).Count();

			return numGraph == 1;
		}
	}
}
