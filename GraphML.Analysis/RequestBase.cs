using System;
using GraphML.Interfaces;

namespace GraphML.Analysis
{
  public abstract class RequestBase : IRequest
  {
    public string Type => GetType().AssemblyQualifiedName;
    public abstract string JobType { get; }
    public Guid CorrelationId { get; set; }
    public Contact Contact { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; }
  }
}
