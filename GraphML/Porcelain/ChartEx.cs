using System.Collections.Generic;

namespace GraphML.Porcelain
{
  public sealed class ChartEx : OwnedItem
  {
    public bool Directed { get; set; } = true;
    public IEnumerable<ChartNodeEx> Nodes { get; set; }
    public IEnumerable<ChartEdgeEx> Edges { get; set; }
  }
}
