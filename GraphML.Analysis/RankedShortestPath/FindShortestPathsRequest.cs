using System;
using GraphML.Interfaces;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsRequest : RequestBase, IFindShortestPathsRequest
  {
    public Guid RootNodeId { get; set; }
    public Guid GoalNodeId { get; set; }

    public override string JobType => typeof(IFindShortestPathsJob).AssemblyQualifiedName;
  }
}
