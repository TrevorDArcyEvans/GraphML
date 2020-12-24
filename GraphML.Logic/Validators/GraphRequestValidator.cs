using System.Linq;
using FluentValidation;
using GraphML.Interfaces;
using GraphML.Logic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GraphML.Logic.Validators
{
	public sealed class GraphRequestValidator : ValidatorBase<IGraphRequest>, IGraphRequestValidator
	{
		private readonly IGraphDatastore _graphDatastore;
		private readonly IRepositoryDatastore _repositoryDatastore;
		private readonly IRepositoryManagerDatastore _repositoryManagerDatastore;
		private readonly IOrganisationDatastore _organisationDatastore;

		public GraphRequestValidator(
		  IHttpContextAccessor context,
		  IContactDatastore contactDatastore,
		  IRoleDatastore roleDatastore,
		  IGraphDatastore graphDatastore,
		  IRepositoryDatastore repositoryDatastore,
		  IRepositoryManagerDatastore repositoryManagerDatastore,
		  IOrganisationDatastore organisationDatastore) :
		  base(context, contactDatastore, roleDatastore)
		{
			_graphDatastore = graphDatastore;
			_repositoryDatastore = repositoryDatastore;
			_repositoryManagerDatastore = repositoryManagerDatastore;
			_organisationDatastore = organisationDatastore;

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

		private bool RequesterIsSameOrganisation(IHttpContextAccessor context, IGraphRequest graphRequest)
		{
			var email = context.Email();
			var contact = _contactDatastore.ByEmail(email);

			var graph = _graphDatastore.ByIds(new[] { graphRequest.GraphId }).Single();
			var repo = _repositoryDatastore.ByIds(new[] { graph.RepositoryId }).Single();
			var repoMgr = _repositoryManagerDatastore.ByIds(new[] { repo.RepositoryManagerId }).Single();
			var org = _organisationDatastore.ByIds(new[] { repoMgr.OrganisationId }).Single();

			return contact.OrganisationId == org.Id;
		}
	}
}
