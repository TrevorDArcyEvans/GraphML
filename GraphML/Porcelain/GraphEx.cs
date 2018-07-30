using System.Collections.Generic;

namespace GraphML.Porcelain
{
  public sealed class GraphEx
  {
    public IEnumerable<Graph> Graphs { get; set; }
    public IEnumerable<GraphItemAttribute> GraphItemAttributes { get; set; }

    public IEnumerable<Node> Nodes { get; set; }
    public IEnumerable<NodeItemAttribute> NodeItemAttributes { get; set; }

    public IEnumerable<Edge> Edges { get; set; }
    public IEnumerable<EdgeItemAttribute> EdgeItemAttributes { get; set; }
  }
}
