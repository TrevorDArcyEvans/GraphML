using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(GraphNode))]
  public sealed class GraphNode : GraphItem
  {
    public GraphNode() :
      base()
    {
    }

    public GraphNode(Guid graph, Guid repositoryItem, string name) :
      base(graph, repositoryItem, name)
    {
    }
  }
}
