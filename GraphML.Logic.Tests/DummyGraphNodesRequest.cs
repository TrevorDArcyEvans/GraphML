using System;
using System.Collections.Generic;
using GraphML.Interfaces;

namespace GraphML.Logic.Tests
{
  public sealed class DummyGraphNodesRequest : IGraphNodesRequest
  {
    public string Type { get; }
    public string JobType { get; }
    public Guid GraphId { get; set; }
    public Guid CorrelationId { get; set; }
    public Contact Contact { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public IEnumerable<Guid> GraphNodes { get; set; }
  }
}
