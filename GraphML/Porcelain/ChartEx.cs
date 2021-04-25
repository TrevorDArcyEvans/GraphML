using System;
using System.Collections.Generic;

namespace GraphML.Porcelain
{
  public sealed class ChartEx : OwnedItem
  {
    public IEnumerable<ChartNodeEx> Nodes { get; set; }
    public IEnumerable<ChartEdgeEx> Edges { get; set; }
  }

  public abstract class ChartItemEx : OwnedItem
  {
    public Guid GraphItemId { get; set; }
    public Guid RepositoryItemId { get; set; }
  }

  public sealed class ChartNodeEx : ChartItemEx
  {
    public int X { get; set; }
    public int Y { get; set; }
  }

  public sealed class ChartEdgeEx : ChartItemEx
  {
    /// <summary>
    /// Unique identifier of source <see cref="Node"/>
    /// Will correspond to a <see cref="ChartItemEx.RepositoryItemId"/> in <see cref="ChartEx.Nodes"/>
    /// </summary>
    public Guid SourceId { get; set; }
    
    /// <summary>
    /// Unique identifier of target <see cref="Node"/>
    /// Will correspond to a <see cref="ChartItemEx.RepositoryItemId"/> in <see cref="ChartEx.Nodes"/>
    /// </summary>
    public Guid TargetId { get; set; }
  }
}
