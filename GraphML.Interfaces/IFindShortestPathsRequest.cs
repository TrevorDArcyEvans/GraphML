using System;

namespace GraphML.Interfaces
{
  public interface IFindShortestPathsRequest : IGraphNodesRequest
  {
    Guid RootNodeId { get; set; }
    Guid GoalNodeId { get; set; }
  }
}
