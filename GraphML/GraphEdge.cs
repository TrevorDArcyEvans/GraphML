using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  [Schema.Table(nameof(GraphEdge))]
  public sealed class GraphEdge : GraphItem
  {
    public GraphEdge() :
      base()
    {
    }

    public GraphEdge(Guid graph, Guid repositoryItem,  string name) :
      base(graph, repositoryItem, name)
    {
    }
  }
}
