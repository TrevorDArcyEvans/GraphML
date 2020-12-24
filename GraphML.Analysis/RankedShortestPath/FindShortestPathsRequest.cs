using System;
using System.Collections.Generic;
using System.Linq;
using GraphML.Interfaces;

namespace GraphML.Analysis.RankedShortestPath
{
  public sealed class FindShortestPathsRequest : RequestBase, IFindShortestPathsRequest
  {
    public Guid RootNodeId { get; set; }
    public Guid GoalNodeId { get; set; }

    public IEnumerable<Guid> GraphNodes
    {
        get => new[] { RootNodeId, GoalNodeId };
        set
        {
            RootNodeId = value.First();
            GoalNodeId = value.Last();
        }
    }

    public override string JobType => typeof(IFindShortestPathsJob).AssemblyQualifiedName;
  }
}
