using System;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace GraphML
{
  /// <summary>
  /// <para>An <see cref="Edge"/> which appears in a <see cref="Graph"/>.</para>
  /// <remarks>Name may be different to that of underlying Edge</remarks>
  /// </summary>
  [Schema.Table(nameof(GraphEdge))]
  public sealed class GraphEdge : GraphItem
  {
    public GraphEdge() :
      base()
    {
    }

    public GraphEdge(Guid graph, Guid orgId, Guid repositoryItem, string name) :
      base(graph, orgId, repositoryItem, name)
    {
    }
  }
}
