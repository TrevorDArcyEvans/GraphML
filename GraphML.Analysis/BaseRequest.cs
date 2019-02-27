namespace GraphML.Analysis
{
  public abstract class RequestBase
  {
    /// <summary>
    /// Type
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Unique reference for this request
    /// </summary>
    public string CorrelationId { get; set; }

    public RequestBase()
    {
      Type = GetType().AssemblyQualifiedName;
    }

    public abstract void Run();
  }
}
