using System;
using GraphML.Interfaces;

namespace GraphML.Logic.Tests
{
  public sealed class DummyGraphRequest : IGraphRequest
  {
    public string Type { get; }
    public string JobType { get; }
    public Guid CorrelationId { get; set; }
    public Contact Contact { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public Guid GraphId { get; set; }
  }
}
