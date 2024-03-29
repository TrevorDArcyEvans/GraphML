using FluentAssertions;
using GraphML.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace GraphML.Logic.Tests.Filters
{
  [TestFixture]
  public sealed class FilterBase_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<IContactDatastore> _contactDatastore;
    private Mock<IRoleDatastore> _roleDatastore;

    [SetUp]
    public void Setup()
    {
      _context = new Mock<IHttpContextAccessor>();
      _contactDatastore = new Mock<IContactDatastore>();
      _roleDatastore = new Mock<IRoleDatastore>();
    }

    [Test]
    public void RequesterMustBeSameOrganisation_SameOrganisation_Succeeds()
    {
      const string email = "DrStrangelove@USAF.com";

      var org = new Organisation();
      var contact = new Contact {OrganisationId = org.Id};
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      var input = new DummyItem {OrganisationId = org.Id};
      var filter = Create();

      var output = filter.Filter(input);

      output.Should().NotBeNull().And.Be(input);
    }

    [Test]
    public void RequesterMustBeAdmin_Admin_Succeeds()
    {
      const string email = "DrStrangelove@USAF.com";

      var role = new Role {Name = "Admin"};
      var contact = new Contact();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      _roleDatastore.Setup(x => x.ByContactId(contact.Id)).Returns(new[] {role});
      var input = new DummyItem();
      var filter = Create();

      var output = filter.Filter(input);

      output.Should().NotBeNull().And.Be(input);
    }

    private DummyFilter Create()
    {
      return new DummyFilter(
        _context.Object,
        _contactDatastore.Object,
        _roleDatastore.Object);
    }
  }
}
