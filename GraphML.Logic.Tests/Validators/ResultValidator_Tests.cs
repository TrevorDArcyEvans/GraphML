using FluentAssertions;
using GraphML.Interfaces;
using GraphML.Logic.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace GraphML.Logic.Tests.Validators
{
  [TestFixture]
	public sealed class ResultValidator_Tests
	{
		private Mock<IHttpContextAccessor> _context;
		private Mock<IContactDatastore> _contactDatastore;
		private Mock<IRoleDatastore> _roleDatastore;
    private Mock<IResultDatastore> _resultDatastore;

		[SetUp]
		public void Setup()
		{
			_context = new Mock<IHttpContextAccessor>();
			_contactDatastore = new Mock<IContactDatastore>();
			_roleDatastore = new Mock<IRoleDatastore>();
      _resultDatastore = new Mock<IResultDatastore>();
		}

		[Test]
		public void MustBeSameContact_SameContact_Succeeds()
		{
			const string email = "DrStrangelove@USAF.com";

			var contact = new Contact();
			_context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
			_contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
			var validator = Create();

			validator.RuleForList();
			var valres = validator.Validate(contact.Id);

			valres.Errors.Should().BeEmpty();
		}

		[Test]
		public void MustBeSameContactAsRequest_SameContacAsRequest_Succeeds()
		{
			const string email = "DrStrangelove@USAF.com";

			var contact = new Contact();
			_context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
			_contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      var request = new DummyRequest { Contact = contact };
      _resultDatastore.Setup(x => x.List(contact.Id)).Returns(new [] { request });
			var validator = Create();

			validator.RuleForRetrieve();
			var valres = validator.Validate(request.CorrelationId);

			valres.Errors.Should().BeEmpty();
		}

		private ResultValidator Create()
		{
			return new ResultValidator(
				_context.Object,
				_contactDatastore.Object,
				_roleDatastore.Object,
				_resultDatastore.Object);
		}
	}
}
