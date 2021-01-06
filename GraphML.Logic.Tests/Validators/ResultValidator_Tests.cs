using System;
using System.Collections.Generic;
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
    public void MustBeSameContactOrUserAdmin_SameContact_ReturnsTrue()
    {
      const string email = "DrStrangelove@USAF.com";

      var contact = new Contact();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      var validator = Create();

      var valres = validator.MustBeSameContactOrUserAdmin(_context.Object, contact.Id);

      valres.Should().BeTrue();
    }

    [Test]
    public void MustBeSameContactOrUserAdmin_UserAdmin_ReturnsTrue()
    {
      const string email = "DrStrangelove@USAF.com";

      var org = new Organisation();
      var reqContact = new Contact { OrganisationId = org.Id };
      var contact = new Contact { OrganisationId = org.Id };
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(reqContact);
      _contactDatastore.Setup(x => x.ByIds(new []{ contact.Id })).Returns(new []{ contact });
      _roleDatastore.Setup(x => x.ByContactId(contact.Id))
            .Returns(new []{ new Role { Name = "UserAdmin" }, new Role { Name = "Other role" } });
      var validator = Create();

      var valres = validator.MustBeSameContactOrUserAdmin(_context.Object, reqContact.Id);

      valres.Should().BeTrue();
    }

    [Test]
    public void MustBeSameOrganisation_SameOrganisation_ReturnsTrue()
    {
      const string email = "DrStrangelove@USAF.com";

      var org = new Organisation();
      var contact = new Contact { OrganisationId = org.Id };
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      var validator = Create();

      var valres = validator.MustBeSameOrganisation(_context.Object, org.Id);

      valres.Should().BeTrue();
    }

    [Test]
    public void RuleForByCorrelation_Succeeds()
    {
      Assert.Pass("TODO   RuleForByCorrelation_Succeeds");
    }

    [Test]
    public void MustBeSameContactAsRequestOrUserAdmin_SameContactAsRequest_ReturnsTrue()
    {
      const string email = "DrStrangelove@USAF.com";

      var contact = new Contact();
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      var request = new DummyRequest { Contact = contact };
      _resultDatastore.Setup(x => x.ByContact(contact.Id)).Returns(new[] { request });
      var validator = Create();

      var valres = validator.MustBeSameContactAsRequestOrUserAdmin(_context.Object,request.CorrelationId);

      valres.Should().BeTrue();
    }

    [Test]
    public void MustBeSameContactAsRequestOrUserAdmin_UserAdmin_ReturnsTrue()
    {
      const string email = "DrStrangelove@USAF.com";

      var org = new Organisation();
      var reqContact = new Contact { OrganisationId = org.Id };
      var contact = new Contact { OrganisationId = org.Id };
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(reqContact);
      _contactDatastore.Setup(x => x.ByIds(new []{ contact.Id })).Returns(new []{ contact });
      var request = new DummyRequest { Contact = reqContact };
      _resultDatastore.Setup(x => x.ByContact(reqContact.Id)).Returns(new[] { request });
      var validator = Create();

      var valres = validator.MustBeSameContactAsRequestOrUserAdmin(_context.Object,request.CorrelationId);

      valres.Should().BeTrue();
    }

    [Test]
    public void IsUserAdmin_IsUserAdmin_ReturnsTrue()
    {
      var contact = new Contact();
      _roleDatastore.Setup(x => x.ByContactId(contact.Id))
            .Returns(new []{ new Role { Name = "UserAdmin" }, new Role { Name = "Other role" } });
      var validator = Create();

      var valres = validator.IsUserAdmin(contact.Id);

      valres.Should().BeTrue();
    }

    [Test]
    public void IsUserAdmin_NotUserAdmin_ReturnsFalse()
    {
      var contact = new Contact();
      _roleDatastore.Setup(x => x.ByContactId(contact.Id))
            .Returns(new []{ new Role { Name = "NotUserAdmin" }, new Role { Name = "Other role" } });
      var validator = Create();

      var valres = validator.IsUserAdmin(contact.Id);

      valres.Should().BeFalse();
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
