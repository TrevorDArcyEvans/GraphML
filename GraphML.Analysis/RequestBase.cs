namespace GraphML.Analysis
{
  public abstract class RequestBase
  {
    /// <summary>
    /// Type
    /// </summary>
    public string Type => GetType().AssemblyQualifiedName;

    public abstract string JobType { get; }

    /// <summary>
    /// Unique reference for this request
    /// </summary>
    public string CorrelationId { get; set; }
  }
}
