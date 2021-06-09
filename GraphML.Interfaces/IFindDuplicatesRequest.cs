namespace GraphML.Interfaces
{
  public interface IFindDuplicatesRequest : IGraphRequest
  {
    int MinMatchingKeyLength { get; set; }
  }
}
