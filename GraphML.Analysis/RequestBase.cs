namespace GraphML.Analysis
{
  public abstract class RequestBase
  {
    /// <summary>
    /// Assembly containing type of request
    /// Used to deserialise request
    /// </summary>
    public string Type => GetType().AssemblyQualifiedName;

    /// <summary>
    /// Assembly implementing IJob which will run this job
    /// </summary>
    public abstract string JobType { get; }

    /// <summary>
    /// Unique reference for this request
    /// </summary>
    public string CorrelationId { get; set; }
  }
}
