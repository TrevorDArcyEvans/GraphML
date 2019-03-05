using GraphML.Interfaces;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsRequest : RequestBase, IFindShortestPathsRequest
  {
    public string RootNodeId { get; set; }
    public string GoalNodeId { get; set; }

    public override string JobType => typeof(IFindShortestPathsJob).AssemblyQualifiedName;
  }
}
