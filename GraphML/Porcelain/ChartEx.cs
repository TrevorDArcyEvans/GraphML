using System.Collections.Generic;

namespace GraphML.Porcelain
{
  public sealed class ChartEx : OwnedItem
  {
    public IEnumerable<ChartNodeEx> Nodes { get; set; }
    public IEnumerable<ChartEdgeEx> Edges { get; set; }
  }
}
