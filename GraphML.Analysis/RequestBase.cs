using System;
using GraphML.Interfaces;

namespace GraphML.Analysis
{
  public abstract class RequestBase : IRequest
  {
    /// <summary>
    /// Unique identifier of graph
    /// </summary>
    public Guid GraphId { get; set; }

    public string Type => GetType().AssemblyQualifiedName;
    public abstract string JobType { get; }
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public Contact Contact { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
  }
}
