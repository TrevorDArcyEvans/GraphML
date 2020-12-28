using System;
using GraphML.Interfaces;

namespace GraphML.Logic.Tests
{
  public sealed class DummyRequest : IRequest
  {
    public string Type => throw new NotImplementedException();

    public string JobType => throw new NotImplementedException();

    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public Contact Contact { get; set; } = new Contact();
    public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime CreatedOnUtc { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  }
}
