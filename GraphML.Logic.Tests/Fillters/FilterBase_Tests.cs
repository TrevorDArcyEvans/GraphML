using System;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;
using GraphML.Interfaces;
using GraphML.Logic.Validators;
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

    [SetUp]
    public void Setup()
    {
      _context = new Mock<IHttpContextAccessor>();
      _contactDatastore = new Mock<IContactDatastore>();
    }

    [Test]
    public void RequesterMustBeSameOrganisation_SameOrganisation_Succeeds()
    {
      const string email = "DrStrangelove@USAF.com";

      var org = new Organisation();
      var contact = new Contact { OrganisationId = org.Id };
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      var input = new DummyItem();
      var filter = Create();

      var output = filter.Filter(input);

      output.Should().NotBeNull().And.Be(input);
    }

    private DummyFilter Create()
    {
      return new DummyFilter(
        _context.Object,
        _contactDatastore.Object);
    }
  }
}
