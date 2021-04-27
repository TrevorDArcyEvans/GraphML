using System;

namespace GraphML.Porcelain
{
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
