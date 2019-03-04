namespace GraphML.Interfaces
{
  public interface IResult
  {
    /// <summary>
    /// Assembly containing type of result
    /// Used to deserialise result
    /// </summary>
    string Type { get; }
  }
}
