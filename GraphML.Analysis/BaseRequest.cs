namespace GraphML.Analysis
{
  public abstract class BaseRequest
  {
    /// <summary>
    /// Type
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Unique reference for this request
    /// </summary>
    public string CorrelationId { get; set; }

    public BaseRequest()
    {
      Type = GetType().AssemblyQualifiedName;
    }

    public abstract void Run();
  }
}
