using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentAssertions;
using GraphML.Interfaces;
using GraphML.Logic.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace GraphML.Logic.Tests.Validators
{
	[TestFixture]
	public sealed class GraphRequestValidator_Tests
	{
		private Mock<IHttpContextAccessor> _context;
		private Mock<IContactDatastore> _contactDatastore;
		private Mock<IRoleDatastore> _roleDatastore;
		private Mock<IGraphDatastore> _graphDatastore;
		private Mock<IRepositoryDatastore> _repositoryDatastore;
		private Mock<IRepositoryManagerDatastore> _repositoryManagerDatastore;
		private Mock<IOrganisationDatastore> _organisationDatastore;

		[SetUp]
		public void Setup()
		{
			_context = new Mock<IHttpContextAccessor>();
			_contactDatastore = new Mock<IContactDatastore>();
			_roleDatastore = new Mock<IRoleDatastore>();
			_graphDatastore = new Mock<IGraphDatastore>();
			_repositoryDatastore = new Mock<IRepositoryDatastore>();
			_repositoryManagerDatastore = new Mock<IRepositoryManagerDatastore>();
			_organisationDatastore = new Mock<IOrganisationDatastore>();
		}

		[Test]
		public void RequesterMustBeSameOrganisation_SameOrganisation_Succeeds()
		{
			const string email = "DrStrangelove@USAF.com";

			var validator = Create();
			var org = new Organisation();
			var repoMgr = new RepositoryManager { OrganisationId = org.Id };
			var repo = new Repository { RepositoryManagerId = repoMgr.Id };
			var graph = new Graph { RepositoryId = repo.Id };
			var contact = new Contact { OrganisationId = org.Id };
			var req = new DummyGraphRequest { GraphId = graph.Id };
			_context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
			_contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
			_graphDatastore.Setup(x => x.ByIds(new[] { req.GraphId })).Returns(new[] { graph });
			_repositoryDatastore.Setup(x => x.ByIds(new[] { graph.RepositoryId })).Returns(new[] { repo });
			_repositoryManagerDatastore.Setup(x => x.ByIds(new[] { repo.RepositoryManagerId })).Returns(new[] { repoMgr });
			_organisationDatastore.Setup(x => x.ByIds(new[] { repoMgr.OrganisationId })).Returns(new[] { org });

			validator.RequesterMustBeSameOrganisation();
			var valres = validator.Validate(req);

			valres.Errors.Should().BeEmpty();
		}

		private GraphRequestValidator Create()
		{
			return new GraphRequestValidator(
				_context.Object,
				_contactDatastore.Object,
				_roleDatastore.Object,
				_graphDatastore.Object,
				_repositoryDatastore.Object,
				_repositoryManagerDatastore.Object,
				_organisationDatastore.Object);
		}
	}
}
