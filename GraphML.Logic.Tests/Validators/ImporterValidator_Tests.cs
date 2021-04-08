using FluentAssertions;
using GraphML.Datastore.Database.Importer;
using GraphML.Interfaces;
using GraphML.Logic.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace GraphML.Logic.Tests.Validators
{
  [TestFixture]
  public sealed class ImporterValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<IContactDatastore> _contactDatastore;
    private Mock<IOrganisationDatastore> _orgDatastore;

    [SetUp]
    public void Setup()
    {
      _context = new Mock<IHttpContextAccessor>();
      _contactDatastore = new Mock<IContactDatastore>();
      _orgDatastore = new Mock<IOrganisationDatastore>();
    }

    [Test]
    public void RequesterIsSameOrganisation_SameOrganisation_ReturnsTrue()
    {
      const string email = "DrStrangelove@USAF.com";

      var org = new Organisation { Name = "US Air Force" };
      var contact = new Contact { OrganisationId = org.Id };
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      _orgDatastore.Setup(x => x.GetAll()).Returns(new[] { org });
      var req = new ImportSpecification { Organisation = org.Name };
      var validator = Create();

      var valres = validator.RequesterIsSameOrganisation(_context.Object, req);

      valres.Should().BeTrue();
    }

    private ImporterValidator Create()
    {
      return new ImporterValidator(
        _context.Object,
        _contactDatastore.Object,
        _orgDatastore.Object);
    }
  }
}
