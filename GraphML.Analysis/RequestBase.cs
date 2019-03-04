using System;
using GraphML.Interfaces;

namespace GraphML.Analysis
{
  public abstract class RequestBase : IRequest
  {
    public string Type => GetType().AssemblyQualifiedName;
    public abstract string JobType { get; }
    public string CorrelationId { get; set; }
    public string ContactId { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; }
  }
}
