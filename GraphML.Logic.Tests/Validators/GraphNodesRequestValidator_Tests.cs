using System;
using System.Linq;
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
  public sealed class GraphNodesRequestValidator_Tests
  {
    private Mock<IHttpContextAccessor> _context;
    private Mock<IContactDatastore> _contactDatastore;
    private Mock<IRoleDatastore> _roleDatastore;
    private Mock<IGraphDatastore> _graphDatastore;
    private Mock<IGraphNodeDatastore> _graphNodeDatastore;

    [SetUp]
    public void Setup()
    {
      _context = new Mock<IHttpContextAccessor>();
      _contactDatastore = new Mock<IContactDatastore>();
      _roleDatastore = new Mock<IRoleDatastore>();
      _graphDatastore = new Mock<IGraphDatastore>();
      _graphNodeDatastore = new Mock<IGraphNodeDatastore>();
    }

    [Test]
    public void RequesterMustBeSameOrganisation_SameOrganisation_Succeeds()
    {
      const string email = "DrStrangelove@USAF.com";

      var org = new Organisation();
      var graph = new Graph { OrganisationId = org.Id };
      var graphNodeRoot = new GraphNode();
      var graphNodeTarget = new GraphNode();
      var graphNodes = new[] { graphNodeRoot, graphNodeTarget };
      var contact = new Contact { OrganisationId = org.Id };
      _context.Setup(x => x.HttpContext).Returns(Creator.GetContext(email));
      _contactDatastore.Setup(x => x.ByEmail(email)).Returns(contact);
      _graphNodeDatastore.Setup(x => x.ByIds(new[] { graphNodeRoot.Id, graphNodeTarget.Id })).Returns(graphNodes);
      _graphDatastore.Setup(x =>
          x.ByIds(It.Is<IEnumerable<Guid>>(gns =>
              gns.First() == graphNodeRoot.GraphId && gns.Last() == graphNodeTarget.GraphId)))
          .Returns(new[] { graph, graph });
      var req = new DummyGraphNodesRequest { GraphNodes = new[] { graphNodeRoot.Id, graphNodeTarget.Id } };
      var validator = Create();

      validator.RequesterMustBeSameOrganisation();
      var valres = validator.Validate(req);

      valres.Errors.Should().BeEmpty();
    }

    [Test]
    public void MustBeInSameGraph_SameGraph_Succeeds()
    {
      var org = new Organisation();
      var graph = new Graph { OrganisationId = org.Id };
      var graphNodeRoot = new GraphNode { GraphId = graph.Id };
      var graphNodeTarget = new GraphNode { GraphId = graph.Id };
      var graphNodes = new[] { graphNodeRoot, graphNodeTarget };
      _graphNodeDatastore.Setup(x => x.ByIds(new[] { graphNodeRoot.Id, graphNodeTarget.Id })).Returns(graphNodes);
      _graphDatastore.Setup(x =>
          x.ByIds(It.Is<IEnumerable<Guid>>(gns =>
              gns.First() == graphNodeRoot.GraphId && gns.Last() == graphNodeTarget.GraphId)))
          .Returns(new[] { graph, graph });
      var req = new DummyGraphNodesRequest { GraphNodes = new[] { graphNodeRoot.Id, graphNodeTarget.Id } };
      var validator = Create();

      validator.MustBeInSameGraph();
      var valres = validator.Validate(req);

      valres.Errors.Should().BeEmpty();
    }

    private GraphNodesRequestValidator Create()
    {
      return new GraphNodesRequestValidator(
        _context.Object,
        _contactDatastore.Object,
        _roleDatastore.Object,
        _graphDatastore.Object,
        _graphNodeDatastore.Object);
    }
  }
}
