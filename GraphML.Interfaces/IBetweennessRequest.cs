namespace GraphML.Interfaces
{
  public interface IBetweennessRequest : IRequest
  {
    string GraphId { get; set; }
  }
}
