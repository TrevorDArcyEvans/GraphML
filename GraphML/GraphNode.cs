using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// <para>A <see cref="Node"/> which appears in a <see cref="Graph"/>.</para>
  /// <remarks>Name may be different to that of underlying Node</remarks>
  /// </summary>
  [Schema.Table(nameof(GraphNode))]
  public sealed class GraphNode : GraphItem
  {
    public GraphNode() :
      base()
    {
    }

    public GraphNode(Guid graph, Guid orgId, Guid repositoryItem, string name) :
      base(graph, orgId, repositoryItem, name)
    {
    }
  }
}
