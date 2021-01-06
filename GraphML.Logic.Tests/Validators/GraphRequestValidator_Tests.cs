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

		[SetUp]
		public void Setup()
		{
			_context = new Mock<IHttpContextAccessor>();
			_contactDatastore = new Mock<IContactDatastore>();
			_roleDatastore = new Mock<IRoleDatastore>();
			_graphDatastore = new Mock<IGraphDatastore>();
		}

		[Test]
		public void RequesterIsSameOrganisation_SameOrganisation_ReturnsTrue()
		{
			const string email = "DrStrangelove@USAF.com";

			var org = new Organisation();
			var graph = new Graph { OrganisationId = org.Id };
			var contact = new Contact { OrganisationId = org.Id };
			_context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
			_contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
			_graphDatastore.Setup(x => x.ByIds(new[] { graph.Id })).Returns(new[] { graph });
			var req = new DummyGraphRequest { GraphId = graph.Id };
			var validator = Create();

			var valres = validator.RequesterIsSameOrganisation(_context.Object, req);

			valres.Should().BeTrue();
		}

		private GraphRequestValidator Create()
		{
			return new GraphRequestValidator(
				_context.Object,
				_contactDatastore.Object,
				_roleDatastore.Object,
				_graphDatastore.Object);
		}
	}
}
