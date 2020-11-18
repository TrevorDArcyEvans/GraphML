using System;

namespace GraphML.Interfaces
{
  public interface IFindShortestPathsRequest : IRequest
  {
    Guid RootNodeId { get; set; }
    Guid GoalNodeId { get; set; }
  }
}
