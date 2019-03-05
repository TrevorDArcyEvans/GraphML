namespace GraphML.Interfaces
{
  public interface IFindShortestPathsRequest : IRequest
  {
    string RootNodeId { get; set; }
    string GoalNodeId { get; set; }
  }
}
